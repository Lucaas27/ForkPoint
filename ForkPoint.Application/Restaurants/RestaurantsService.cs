using AutoMapper;
using ForkPoint.Application.Restaurants.DTOs;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Restaurants;
internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDTO>> GetAllAsync()
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantsDTO = mapper.Map<IEnumerable<RestaurantDTO>>(restaurants);

        return restaurantsDTO;
    }

    public async Task<RestaurantDTO?> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting restaurant by id: {id}");

        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        var restaurantDTO = mapper.Map<RestaurantDTO?>(restaurant);

        return restaurantDTO;
    }
}
