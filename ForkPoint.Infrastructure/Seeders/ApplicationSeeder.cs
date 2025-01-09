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
        }
    }

    private static List<User> GetAdmins()
    {
        var admins = new List<User>
        {
            new()
            {
                Email = "admin@forkpoint.com", EmailConfirmed = true, UserName = "admin@forkpoint.com",
                FullName = "Admin"
            }
        };

        return admins;
    }

    private static List<IdentityRole<int>> GetRoles()
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