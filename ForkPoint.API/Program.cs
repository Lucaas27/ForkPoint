using ForkPoint.API.Extensions;
using ForkPoint.API.Middlewares;
using ForkPoint.Application.Extensions;
using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Extensions;
using ForkPoint.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Add services to the container.
builder.AddPresentation();
services.AddInfrastructure(config);
services.AddApplication(config);

// Build the app
var app = builder.Build();
var scope = app.Services.CreateScope(); // Create a scope to resolve services from the container
await scope.ServiceProvider.GetRequiredService<ISeeder>().Seed(); // Seed the database

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ForkPoint API");
    options.RoutePrefix = string.Empty;
});
app.UseMiddleware<ElapsedTimeMiddleware>();
app.UseSerilogRequestLogging();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
