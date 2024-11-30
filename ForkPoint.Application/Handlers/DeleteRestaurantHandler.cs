using AutoMapper;
using ForkPoint.Application.Models.Handlers.DeleteRestaurant;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
/// <summary>
/// Handles the deletion of a restaurant.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="mapper">The mapper instance for mapping objects.</param>
/// <param name="restaurantsRepository">The repository instance for accessing restaurant data.</param>
public class DeleteRestaurantHandler(
    ILogger<DeleteRestaurantHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : BaseHandler<DeleteRestaurantRequest, DeleteRestaurantResponse>(logger, mapper, restaurantsRepository)
{
    /// <summary>
    /// Handles the request to delete a restaurant by its ID.
    /// </summary>
    /// <param name="request">The request containing the ID of the restaurant to delete.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A response indicating the success of the deletion operation.</returns>
    /// <exception cref="NotFoundException">Thrown when the restaurant with the specified ID is not found.</exception>
    public override async Task<DeleteRestaurantResponse> Handle(DeleteRestaurantRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Deleting restaurant id...");

        var restaurant = await _restaurantsRepository.GetRestaurantByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id);

        await _restaurantsRepository.DeleteRestaurant(restaurant);

        _logger.LogInformation($"Restaurant with id {request.Id} deleted.");

        return new DeleteRestaurantResponse
        {
            IsSuccess = true,
            Message = $"Restaurant with id {request.Id} deleted."
        };
    }
}
