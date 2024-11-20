using ForkPoint.Domain.Entities;

namespace ForkPoint.Domain.Repositories;
public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> CreateAsync(Restaurant entity);
    //Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
    //Task DeleteRestaurantAsync(int id);

}
