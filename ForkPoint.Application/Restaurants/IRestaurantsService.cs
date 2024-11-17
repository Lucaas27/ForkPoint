using ForkPoint.Application.Restaurants.DTOs;

namespace ForkPoint.Application.Restaurants;
public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDTO>> GetAllAsync();
    Task<RestaurantDTO?> GetByIdAsync(int id);
}