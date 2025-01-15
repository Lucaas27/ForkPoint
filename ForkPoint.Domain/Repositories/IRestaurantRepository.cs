using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task<int> CreateRestaurantAsync(Restaurant entity);
    Task UpdateDb();
    Task DeleteRestaurant(Restaurant restaurant);
    Task<(IEnumerable<Restaurant>, int)> GetFilteredRestaurantsAsync(string? searchTerm, int pageNumber, int pageSize);
}