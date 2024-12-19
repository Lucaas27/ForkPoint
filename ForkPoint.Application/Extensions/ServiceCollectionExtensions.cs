using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using ForkPoint.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ForkPoint.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(c => c.RegisterServicesFromAssembly(applicationAssembly));

        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        services.AddSingleton<IAuthService, AuthService>();

        services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    LogValidationExceptions = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = config["Jwt:Issuer"]
                                  ?? throw new ArgumentNullException(nameof(config), "Jwt:Issuer is null"),
                    ValidAudience = config["Jwt:Audience"]
                                    ?? throw new ArgumentNullException(nameof(config),
                                        "Authentication:Audience is null"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["Jwt:Key"]
                                               ?? throw new ArgumentNullException(nameof(config),
                                                   "Authentication:Secret is null"))
                    )
                };
            })
            .AddCookie() // For temporary state during External provider authentication
            .AddGoogle(options =>
            {
                options.ClientId = config["Authentication:Google:ClientId"]
                                   ?? throw new ArgumentNullException(nameof(config),
                                       "Authentication:Google:ClientId is null");
                options.ClientSecret = config["Authentication:Google:ClientSecret"]
                                       ?? throw new ArgumentNullException(nameof(config),
                                           "Authentication:Google:ClientSecret is null");

                // Use cookies for Google sign-in
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });
    }
}