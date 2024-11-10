using ForkPoint.API.Controllers;
using ForkPoint.Infrastructure.Extensions;
using ForkPoint.Infrastructure.Seeders;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
// Extension method to add infrastructure services.
builder.Services.AddInfrastructure(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ForkPoint API",
        Description = "An ASP.NET Core Web API for managing restaurants and menus",
        //TermsOfService = new Uri("https://example.com/terms"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Example Contact",
        //    Url = new Uri("https://example.com/contact")
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Example License",
        //    Url = new Uri("https://example.com/license")
        //}
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();
var scope = app.Services.CreateScope(); // Create a scope to resolve services from the container 
await scope.ServiceProvider.GetRequiredService<ISeeder>().Seed(); // Seed the database

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ForkPoint API");
        options.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
