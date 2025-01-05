using ForkPoint.Application.Extensions;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;
using ForkPoint.Infrastructure.Repositories;
using ForkPoint.Infrastructure.Seeders;
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
    }
}