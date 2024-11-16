using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Restaurants;
public interface IRestaurantsService
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
}