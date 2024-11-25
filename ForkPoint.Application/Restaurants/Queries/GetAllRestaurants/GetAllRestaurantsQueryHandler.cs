using AutoMapper;
using ForkPoint.Application.Models.Restaurant;
using ForkPoint.Application.Restaurants.Commands.NewRestaurant;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Restaurants.Queries.GetAllRestaurants;
public class GetAllRestaurantsQueryHandler(ILogger<NewRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantModel>>
{

    public async Task<IEnumerable<RestaurantModel>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllAsync();
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantModel>>(restaurants);

        return restaurantsDto;
    }
}



