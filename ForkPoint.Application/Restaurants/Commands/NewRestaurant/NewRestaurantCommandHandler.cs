using AutoMapper;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Restaurants.Commands.NewRestaurant;
public class NewRestaurantCommandHandler(ILogger<NewRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository) : IRequestHandler<NewRestaurantCommand, int>
{
    public async Task<int> Handle(NewRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new restaurant...");

        var restaurant = mapper.Map<Restaurant>(request);
        var id = await restaurantsRepository.CreateAsync(restaurant);

        return id;
    }
}
