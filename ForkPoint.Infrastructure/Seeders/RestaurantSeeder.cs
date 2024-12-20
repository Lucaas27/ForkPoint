﻿using System.Text.Json;
using ForkPoint.Domain.Entities;
using ForkPoint.Infrastructure.Persistence;

namespace ForkPoint.Infrastructure.Seeders;

internal class RestaurantSeeder(ApplicationDbContext dbContext) : ISeeder
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
    }

    private static async Task<IEnumerable<Restaurant>> GetRestaurantListAsync()
    {
        var jsonData = await File.ReadAllTextAsync(JsonFilePath);
        var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(jsonData);
        return restaurants ?? [];
    }
}