using AutoMapper;
using ForkPoint.Application.Models.Handlers.NewRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
/// Handler for creating a new restaurant.
/// </summary>
/// <param name="logger">Logger instance for logging information.</param>
/// <param name="mapper">Mapper instance for mapping request to entity.</param>
/// <param name="restaurantsRepository">Repository instance for restaurant operations.</param>
public class NewRestaurantHandler(
    ILogger<NewRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<NewRestaurantRequest, NewRestaurantResponse>(logger, mapper, restaurantsRepository)
{
    /// <summary>
    /// Handles the creation of a new restaurant.
    /// </summary>
    /// <param name="request">The request containing restaurant details.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A response indicating the success of the operation and the new record ID.</returns>
    public override async Task<NewRestaurantResponse> Handle(NewRestaurantRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
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
