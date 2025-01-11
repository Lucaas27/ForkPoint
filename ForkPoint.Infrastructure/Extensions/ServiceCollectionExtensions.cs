using ForkPoint.Application.Extensions;
using ForkPoint.Domain.Constants;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Authorization.Handlers;
using ForkPoint.Infrastructure.Authorization.Requirements;
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
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                    options.Tokens.PasswordResetTokenProvider = "CustomPasswordTokenProvider";
                }
            )
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddCustomPasswordTokenProvider()
            .AddCustomRefreshTokenProvider();

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                5,
                TimeSpan.FromSeconds(30),
                null);
        }));
        services.AddScoped<ISeeder, ApplicationSeeder>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IAuthorizationHandler, OwnsRestaurantOrAdminHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy(AppPolicies.OwnerPolicy, policy => policy.RequireRole(AppPolicies.OwnerPolicy))
            .AddPolicy(AppPolicies.AdminPolicy, policy => policy.RequireRole(AppPolicies.AdminPolicy))
            .AddPolicy(AppPolicies.UserPolicy, policy => policy.RequireRole(AppPolicies.UserPolicy))
            .AddPolicy(AppPolicies.AdminOrOwnerPolicy,
                policy => policy.RequireRole(AppPolicies.AdminPolicy, AppPolicies.OwnerPolicy))
            .AddPolicy(AppPolicies.OwnsRestaurantOrAdminPolicy,
                policy => policy.AddRequirements(new OwnsRestaurantOrAdminRequirement()));
    }
}