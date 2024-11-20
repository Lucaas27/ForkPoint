using ForkPoint.Application.Models.Restaurant;

namespace ForkPoint.Application.Services.Restaurant;
public interface IRestaurantService
{
    Task<IEnumerable<RestaurantModel>> GetAllAsync();
    Task<RestaurantDetailsModel?> GetByIdAsync(int id);
}