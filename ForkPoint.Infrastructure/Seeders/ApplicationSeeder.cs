using System.Text.Json;
using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ForkPoint.Infrastructure.Seeders;

internal class ApplicationSeeder(ApplicationDbContext dbContext, UserManager<User> userManager) : ISeeder
{
    private static readonly string JsonFilePath = Path.Combine(AppContext.BaseDirectory, "Seeders", "restaurants.json");

    public async Task Seed()
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurantList = await GetRestaurantListAsync();
                dbContext.Restaurants.AddRange(restaurantList);
                await dbContext.SaveChangesAsync();
            }
        }

        // Default users
        var administrator = new User
        {
            Email = "admin@forkpoint.com", EmailConfirmed = true, UserName = "admin@forkpoint.com", FullName = "Admin"
        };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "AdminPassword1!");
            await userManager.AddToRolesAsync(administrator, ["Admin"]);
        }
    }

    private static async Task<IEnumerable<Restaurant>> GetRestaurantListAsync()
    {
        var jsonData = await File.ReadAllTextAsync(JsonFilePath);
        var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(jsonData);
        return restaurants ?? [];
    }
}