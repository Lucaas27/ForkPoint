using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using ForkPoint.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForkPoint.Infrastructure.Repositories;
internal class RestaurantRepository(ApplicationDbContext dbContext)
    : IRestaurantRepository
{
    public async Task<int> CreateRestaurantAsync(Restaurant entity)
    {
        await dbContext.Restaurants.AddAsync(entity);
        await UpdateDb();

        return entity.Id;
    }

    public async Task DeleteRestaurant(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await UpdateDb();
    }

    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Address)
            .Include(r => r.MenuItems)
            .FirstOrDefaultAsync(r => r.Id == id);

        return restaurant;
    }

    public async Task UpdateDb() => await dbContext.SaveChangesAsync();


}
