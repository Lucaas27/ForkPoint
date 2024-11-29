using AutoMapper;
using ForkPoint.Application.Models.Handlers.NewRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class NewRestaurantHandler(
    ILogger<NewRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<NewRestaurantRequest, NewRestaurantResponse>(logger, mapper, restaurantsRepository)
{
    public override async Task<NewRestaurantResponse> Handle(NewRestaurantRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new restaurant...");

        var restaurant = _mapper.Map<Restaurant>(request);
        var id = await _restaurantsRepository.CreateAsync(restaurant);

        return new NewRestaurantResponse
        {
            IsSuccess = true,
            NewRecordId = id
        };
    }
}
