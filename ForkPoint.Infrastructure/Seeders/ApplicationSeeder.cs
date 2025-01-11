using System.Text.Json;
using ForkPoint.Domain.Constants;
using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Infrastructure.Seeders;

internal class ApplicationSeeder(
    ApplicationDbContext dbContext,
    UserManager<User> userManager
) : ISeeder
{
    private static readonly string JsonFilePath = Path.Combine(AppContext.BaseDirectory, "Seeders", "restaurants.json");

    public async Task Seed()
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (await dbContext.Database.CanConnectAsync())
        {
            // Seed default restaurants
            if (!dbContext.Restaurants.Any())
            {
                var restaurantList = await GetRestaurantListAsync();
                dbContext.Restaurants.AddRange(restaurantList);
                await dbContext.SaveChangesAsync();
            }

            // Seed default roles
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }

            // Seed Default admins
            var admins = GetAdmins();
            if (userManager.Users.All(u => !admins.Select(a => a.Email).Contains(u.Email)))
            {
                foreach (var user in admins)
                {
                    await userManager.CreateAsync(user, "AdminPassword1!");
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }


            // Seed Default users
            var users = GetUsers();
            if (userManager.Users.All(u => !users.Select(a => a.Email).Contains(u.Email)))
            {
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "UserPassword1!");
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
        }
    }

    private static IEnumerable<User> GetUsers()
    {
        var users = new List<User>
        {
            new()
            {
                Email = "forkpointuser@gmail.com", EmailConfirmed = true, UserName = "forkpointuser@gmail.com",
                FullName = "ForkPoint User"
            }
        };

        return users;
    }

    private static IEnumerable<User> GetAdmins()
    {
        var admins = new List<User>
        {
            new()
            {
                Email = "forkpointadmin@gmail.com", EmailConfirmed = true, UserName = "forkpointadmin@gmail.com",
                FullName = "ForkPoint Admin"
            }
        };

        return admins;
    }

    private static IEnumerable<IdentityRole<int>> GetRoles()
    {
        var roles = new List<IdentityRole<int>>
        {
            new()
            {
                Name = AppUserRoles.Admin,
                NormalizedName = AppUserRoles.Admin.ToUpperInvariant(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new()
            {
                Name = AppUserRoles.Owner,
                NormalizedName = AppUserRoles.Owner.ToUpperInvariant(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new()
            {
                Name = AppUserRoles.User,
                NormalizedName = AppUserRoles.User.ToUpperInvariant(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        };

        return roles;
    }

    private static async Task<IEnumerable<Restaurant>> GetRestaurantListAsync()
    {
        var jsonData = await File.ReadAllTextAsync(JsonFilePath);
        var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(jsonData);
        return restaurants ?? [];
    }
}