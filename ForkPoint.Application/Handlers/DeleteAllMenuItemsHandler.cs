using AutoMapper;
using ForkPoint.Application.Models.Handlers.DeleteAllMenuItems;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;


/// <summary>
/// Handler for deleting all menu items for a specific restaurant.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="mapper">The AutoMapper instance for mapping objects.</param>
/// <param name="restaurantRepository">The repository for accessing restaurant data.</param>
/// <param name="menuRepository">The repository for accessing menu data.</param>
public class DeleteAllMenuItemsHandler(
    ILogger<DeleteAllMenuItemsHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository,
    IMenuRepository menuRepository)
    : BaseHandler<DeleteAllMenuItemsRequest, DeleteAllMenuItemsResponse>(logger, mapper, restaurantRepository, menuRepository)
{
    public override async Task<DeleteAllMenuItemsResponse> Handle(DeleteAllMenuItemsRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {@Request}", request);
        logger.LogInformation("Deleting all menu items for restaurant id {RestaurantId}", request.RestaurantId);

        var restaurant = await _restaurantsRepository!.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId);

        await _menuRepository!.DeleteAllMenuItemsAsync(restaurant.MenuItems);

        return new DeleteAllMenuItemsResponse
        {
            IsSuccess = true,
            Message = string.Format("All menu items for restaurant {0} were deleted", request.RestaurantId)
        };
    }
}
