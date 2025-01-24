using AutoMapper;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Handlers.CreateRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Repositories;
using MediatR;
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
    IRestaurantRepository restaurantsRepository,
    IUserContext userContext
)
    : IRequestHandler<CreateRestaurantRequest, CreateRestaurantResponse>
{
    /// <summary>
    ///     Handles the creation of a new restaurant.
    /// </summary>
    /// <param name="request">The request containing restaurant details.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A response indicating the success of the operation and the new record ID.</returns>
    public async Task<CreateRestaurantResponse> Handle(
        CreateRestaurantRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetCurrentUser();
        var userId = user?.Id ?? throw new InvalidOperationException("User not found");

        logger.LogInformation("User {User} - {UserId} is creating a new restaurant...", user.Email, userId);

        var restaurant = mapper.Map<Restaurant>(request);

        restaurant.OwnerId = userId;

        var restaurantId = await restaurantsRepository.CreateRestaurantAsync(restaurant);

        return new CreateRestaurantResponse(restaurantId)
        {
            IsSuccess = true
        };
    }
}