using ForkPoint.Application.Extensions;
using ForkPoint.Application.ExternalClients.Foursquare;
using ForkPoint.Domain.Constants;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Authorization.Handlers;
using ForkPoint.Infrastructure.Authorization.Requirements;
using ForkPoint.Infrastructure.ExternalClients.Foursquare;
using ForkPoint.Infrastructure.Persistence;
using ForkPoint.Infrastructure.Repositories;
using ForkPoint.Infrastructure.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForkPoint.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Default");
        services.AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider =
                        TokenOptions.DefaultEmailProvider;

                    options.Tokens.PasswordResetTokenProvider = "CustomPasswordTokenProvider";
                }
            )
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddCustomPasswordTokenProvider()
            .AddCustomRefreshTokenProvider();

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    5,
                    TimeSpan.FromSeconds(30),
                    null);
            }));

        services.AddScoped<ISeeder, ApplicationSeeder>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthorizationHandler, OwnsRestaurantOrAdminHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy(AppPolicies.AdminPolicy, policy => policy.RequireRole(AppUserRoles.Admin))
            .AddPolicy(AppPolicies.AdminOrOwnerPolicy,
                policy => policy.RequireRole(AppUserRoles.Admin, AppUserRoles.Owner))
            .AddPolicy(AppPolicies.OwnsRestaurantOrAdminPolicy,
                policy => policy.AddRequirements(new OwnsRestaurantOrAdminRequirement()));

        services.AddScoped<IFoursquareClient, FoursquareClient>();

        services.AddHttpClient("FoursquareClient",
            (serviceProvider, client) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var apiKey = configuration["Apis:Foursquare:ApiKey"] ??
                             throw new InvalidOperationException(
                                 "Foursquare ApiKey is not configured");

                var baseUrl = configuration["Apis:Foursquare:BaseUrl"] ??
                              throw new InvalidOperationException(
                                  "Foursquare BaseUrl is not configured");

                var apiVersion = configuration["Apis:Foursquare:Version"] ??
                                 throw new InvalidOperationException(
                                     "Foursquare version is not configured");

                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                client.DefaultRequestHeaders.Add("X-Places-Api-Version", apiVersion);

                client.DefaultRequestHeaders.Add("Accept", "application/json");

                client.Timeout = TimeSpan.FromSeconds(15);
            });
    }
}
