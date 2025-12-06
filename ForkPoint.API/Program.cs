using ForkPoint.API.Extensions;
using ForkPoint.API.Middlewares;
using ForkPoint.Application.Extensions;
using ForkPoint.Infrastructure.Extensions;
using ForkPoint.Infrastructure.Seeders;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
services.AddInfrastructure(config);
services.AddApplication(config);
builder.AddPresentation();

// Build the app
var app = builder.Build();
// Create a scope to resolve services from the container
using var scope = app.Services.CreateScope();

// Apply any pending EF Core migrations then seed the database and log results
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    logger.LogInformation("Applying EF Core migrations...");
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    logger.LogInformation("Migrations applied.");

    logger.LogInformation("Running database seeder...");
    await scope.ServiceProvider.GetRequiredService<ISeeder>().Seed(); // Seed the database
    logger.LogInformation("Seeding finished.");

    try
    {
        var rolesCount = db.Roles.Count();
        var usersCount = db.Users.Count();
        var restaurantsCount = db.Restaurants.Count();
        logger.LogInformation("DB counts after seed: Roles={roles}, Users={users}, Restaurants={restaurants}", rolesCount, usersCount, restaurantsCount);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Could not read counts from database after seeding.");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
}

// Add Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("CorsPolicy");

app.UseSwagger()
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ForkPoint API");
        options.EnableTryItOutByDefault();
        options.RoutePrefix = string.Empty;
    })
    .UseSerilogRequestLogging()
    .UseMiddleware<ElapsedTimeMiddleware>()
    .UseMiddleware<ErrorHandlerMiddleware>()
    .UseAuthentication()
    .UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseHttpLogging();
}

app.MapControllers();

// Run the app
await app.RunAsync();
