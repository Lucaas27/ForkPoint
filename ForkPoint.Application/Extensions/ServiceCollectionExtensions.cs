using FluentValidation;
using FluentValidation.AspNetCore;
using ForkPoint.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForkPoint.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(config => config.RegisterServicesFromAssembly(applicationAssembly));

        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        services.AddAuthentication(
        // options =>
        // {
        //     options.DefaultAuthenticateScheme =
        //     options.DefaultChallengeScheme =
        //     options.DefaultForbidScheme =
        //     options.DefaultScheme =
        //     options.DefaultSignInScheme =
        //     options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        // }
        )
            .AddCookie() // For temporary state during External provider authentication
            .AddGoogle(options =>
            {
                options.ClientId = config["Authentication:Google:ClientId"]
                    ?? throw new ArgumentNullException(nameof(options.ClientId), "Authentication:Google:ClientId is null");
                options.ClientSecret = config["Authentication:Google:ClientSecret"]
                    ?? throw new ArgumentNullException(nameof(options.ClientSecret), "Authentication:Google:ClientSecret is null");

                // Use cookies for Google sign-in
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });


        services.AddScoped<ITokenService, TokenService>();
    }
}
