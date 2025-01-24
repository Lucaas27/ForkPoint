using ForkPoint.Application.Models.Handlers.DeleteAllMenuItems;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
///     Handler for deleting all menu items for a specific restaurant.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="restaurantRepository">The repository for accessing restaurant data.</param>
/// <param name="menuRepository">The repository for accessing menu data.</param>
public class DeleteAllMenuItemsHandler(
    ILogger<DeleteAllMenuItemsHandler> logger,
    IRestaurantRepository restaurantRepository,
    IMenuRepository menuRepository
)
    : IRequestHandler<DeleteAllMenuItemsRequest, DeleteAllMenuItemsResponse>
{
    public async Task<DeleteAllMenuItemsResponse> Handle(
        DeleteAllMenuItemsRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Deleting all menu items for restaurant id {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId)
                         ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        await menuRepository.DeleteAllMenuItemsAsync(restaurant.MenuItems);

        return new DeleteAllMenuItemsResponse
        {
            IsSuccess = true,
            Message = $"All menu items for restaurant {request.RestaurantId} were deleted"
        };
    }
}