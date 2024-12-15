using AutoMapper;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
///     Handler for creating a new restaurant.
/// </summary>
/// <param name="logger">Logger instance for logging information.</param>
/// <param name="mapper">Mapper instance for mapping request to entity.</param>
/// <param name="restaurantsRepository">Repository instance for restaurant operations.</param>
public class CreateRestaurantHandler(
    ILogger<CreateRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<CreateRestaurantRequest, CreateRestaurantResponse>
{
    /// <summary>
    ///     Handles the creation of a new restaurant.
    /// </summary>
    /// <param name="request">The request containing restaurant details.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A response indicating the success of the operation and the new record ID.</returns>
    public override async Task<CreateRestaurantResponse> Handle(CreateRestaurantRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new restaurant...");

        var restaurant = mapper.Map<Restaurant>(request);
        var id = await restaurantsRepository.CreateRestaurantAsync(restaurant);

        return new CreateRestaurantResponse(id)
        {
            IsSuccess = true
        };
    }
}