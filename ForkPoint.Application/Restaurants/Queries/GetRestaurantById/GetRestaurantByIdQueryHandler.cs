using AutoMapper;
using ForkPoint.Application.Models.Restaurant;
using ForkPoint.Application.Restaurants.Commands.NewRestaurant;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Restaurants.Queries.GetRestaurantById;
public class GetRestaurantByIdQueryHandler(ILogger<NewRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDetailsModel?>
{
    public async Task<RestaurantDetailsModel?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Getting restaurant by id: {request.Id}");

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        var restaurantDTO = mapper.Map<RestaurantDetailsModel?>(restaurant);

        return restaurantDTO;
    }
}
