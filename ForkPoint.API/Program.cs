using ForkPoint.API.Middlewares;
using ForkPoint.Application.Extensions;
using ForkPoint.Infrastructure.Extensions;
using ForkPoint.Infrastructure.Seeders;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ForkPoint API",
        Description = "ASP.NET Core Web API for managing restaurants and menus",
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
builder.Services.AddScoped<ErrorHandlerMiddleware>();
builder.Services.AddScoped<ElapsedTimeMiddleware>();
builder.Services.AddInfrastructure(config);
builder.Services.AddApplication();
builder.Host.UseSerilog((context, config) =>
{
    config
    .ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddEndpointsApiExplorer();



// Build the app
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

app.UseMiddleware<ElapsedTimeMiddleware>();
app.UseSerilogRequestLogging();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
