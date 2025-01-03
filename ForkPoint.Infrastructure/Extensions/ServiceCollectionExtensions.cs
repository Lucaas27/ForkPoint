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
        services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                    options.Tokens.PasswordResetTokenProvider = "PasswordResetTokenProvider";
                }
            )
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<CustomPasswordResetTokenProvider<User>>("PasswordResetTokenProvider");

        // Set the lifespan of the reset password token to be 10 min
        services.Configure<CustomPasswordResetTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromMinutes(10));

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ISeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
    }
}