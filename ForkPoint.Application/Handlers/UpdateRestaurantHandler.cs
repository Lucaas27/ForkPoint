using AutoMapper;
using ForkPoint.Application.Models.Handlers.UpdateRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
///     Handler for updating restaurant information.
/// </summary>
/// <param name="logger">Logger instance for logging information.</param>
/// <param name="mapper">Mapper instance for mapping request data to domain model.</param>
/// <param name="restaurantRepository">Repository instance for accessing restaurant data.</param>
public class UpdateRestaurantHandler(
    ILogger<UpdateRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository
)
    : BaseHandler<UpdateRestaurantRequest, UpdateRestaurantResponse>
{
    /// <summary>
    ///     Handles the update restaurant request.
    /// </summary>
    /// <param name="request">The update restaurant request containing the restaurant data to be updated.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Response indicating the success or failure of the update operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the restaurant with the specified ID is not found.</exception>
    public override async Task<UpdateRestaurantResponse> Handle(
        UpdateRestaurantRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Updating restaurant id {@RestaurantId}...", request.Id);
        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.Id)
                         ?? throw new NotFoundException(nameof(Restaurant), request.Id);

        // Map the request data to the domain model
        mapper.Map(request, restaurant);

        await restaurantRepository.UpdateDb();

        return new UpdateRestaurantResponse
        {
            IsSuccess = true,
            Message = $"Restaurant id {request.Id} updated successfully."
        };
    }
}