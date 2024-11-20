using ForkPoint.Application.Models.Restaurant;

namespace ForkPoint.Application.Services.Restaurants;
public interface IRestaurantService
{
    Task<IEnumerable<RestaurantModel>> GetAllAsync();
    Task<RestaurantDetailsModel?> GetByIdAsync(int id);
    Task<int> CreateAsync(NewRestaurantModel newRestaurant);
}