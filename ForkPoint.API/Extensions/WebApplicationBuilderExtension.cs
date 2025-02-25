﻿using System.Reflection;
using System.Text.Json.Serialization;
using ForkPoint.API.Middlewares;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ForkPoint.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config
                .ReadFrom.Configuration(context.Configuration);
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.ResponseBody |
                                    HttpLoggingFields.ResponseStatusCode |
                                    HttpLoggingFields.RequestBody |
                                    HttpLoggingFields.RequestMethod |
                                    HttpLoggingFields.RequestPath;

            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
            logging.CombineLogs = true;

            // Add sensitive headers to be masked
            logging.MediaTypeOptions.AddText("application/json");
            logging.RequestHeaders.Add("Authorization");
            logging.RequestHeaders.Add("X-Api-Key");
        });

        builder.Services.AddScoped<ErrorHandlerMiddleware>();

        builder.Services.AddScoped(_ => new SensitiveDataLoggingMiddleware(builder.Configuration));

        builder.Services.AddScoped<ElapsedTimeMiddleware>();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Description = "Example \"{token}\"",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });

            options.DescribeAllParametersInCamelCase();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ForkPoint API",
                Description = "ASP.NET 8 Core Web API for managing restaurants and menus"

            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }
}