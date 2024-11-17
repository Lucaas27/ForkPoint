using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;
public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>?> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    //Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);
    //Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
    //Task DeleteRestaurantAsync(int id);

}
