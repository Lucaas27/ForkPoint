using AutoMapper;
using ForkPoint.Application.Models.Restaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Services.Restaurants;
internal class RestaurantService(IRestaurantRepository restaurantsRepository,
    ILogger<RestaurantService> logger,
    IMapper mapper)
    : IRestaurantService
{
    public async Task<IEnumerable<RestaurantModel>> GetAllAsync()
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantsDTO = mapper.Map<IEnumerable<RestaurantModel>>(restaurants);

        return restaurantsDTO;
    }

    public async Task<RestaurantDetailsModel?> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting restaurant by id: {id}");

        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        var restaurantDTO = mapper.Map<RestaurantDetailsModel?>(restaurant);

        return restaurantDTO;
    }

    public async Task<int> CreateAsync(NewRestaurantModel newRestaurant)
    {
        logger.LogInformation("Creating new restaurant...");

        var restaurant = mapper.Map<Restaurant>(newRestaurant);
        var id = await restaurantsRepository.CreateAsync(restaurant);

        return id;
    }
}
