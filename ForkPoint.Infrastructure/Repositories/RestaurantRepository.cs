using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Repositories;
internal class RestaurantRepository(ForkPointDbContext dbContext)
    : IRestaurantRepository
{
    public async Task<int> CreateAsync(Restaurant entity)
    {
        await dbContext.Restaurants.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants
            .ToListAsync();

        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Address)
            .Include(r => r.MenuItems)
            .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }
}
