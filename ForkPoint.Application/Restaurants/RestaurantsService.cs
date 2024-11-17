using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Restaurants;
internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger) : IRestaurantsService
{
    public async Task<IEnumerable<Restaurant>?> GetAllAsync()
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllAsync();

        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting restaurant by id: {id}");

        var restaurant = await restaurantsRepository.GetByIdAsync(id);

        return restaurant;
    }
}
