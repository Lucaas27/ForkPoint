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
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    // options.Password.RequireDigit = false;
                    // options.Password.RequireLowercase = false;
                    // options.Password.RequireNonAlphanumeric = false;
                    // options.Password.RequireUppercase = false;
                    // options.Password.RequiredLength = 3;
                    // options.User.RequireUniqueEmail = true;
                    // options.SignIn.RequireConfirmedEmail = true;
                    // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    // options.Lockout.MaxFailedAccessAttempts = 5;
                    // options.Lockout.AllowedForNewUsers = true;
                    // options.Tokens.EmailConfirmationTokenProvider
                    // options.Tokens.ChangeEmailTokenProvider
                    // options.Tokens.ChangePhoneNumberTokenProvider
                    // options.Tokens.PasswordResetTokenProvider
                }
            )
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ISeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
    }
}