using ForkPoint.API.Extensions;
using ForkPoint.API.Middlewares;
using ForkPoint.Application.Extensions;
using ForkPoint.Infrastructure.Extensions;
using ForkPoint.Infrastructure.Seeders;
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
var scope = app.Services.CreateScope(); // Create a scope to resolve services from the container
await scope.ServiceProvider.GetRequiredService<ISeeder>().Seed(); // Seed the database

// Add Middleware
app.UseHttpsRedirection()
    .UseSwagger()
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
