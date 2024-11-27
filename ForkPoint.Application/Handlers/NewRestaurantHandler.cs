using AutoMapper;
using ForkPoint.Application.Models.Handlers.NewRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class NewRestaurantHandler(ILogger<NewRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : IRequestHandler<NewRestaurantRequest, NewRestaurantResponse>
{

    public async Task<NewRestaurantResponse> Handle(NewRestaurantRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new restaurant...");

        var restaurant = mapper.Map<Restaurant>(request);
        var id = await restaurantsRepository.CreateAsync(restaurant);

        return new NewRestaurantResponse
        {
            IsSuccess = true,
            NewRecordId = id
        };
    }
}
