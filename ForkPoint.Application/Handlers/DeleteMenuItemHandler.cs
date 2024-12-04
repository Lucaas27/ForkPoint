using AutoMapper;
using ForkPoint.Application.Models.Handlers.DeleteMenuItem;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
/// Handles the deletion of a specific menu item from a restaurant.
/// </summary>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
/// <param name="restaurantsRepository">The repository for accessing restaurant data.</param>
/// <param name="menuRepository">The repository for accessing menu data.</param>
public class DeleteMenuItemHandler(
    ILogger<DeleteMenuItemHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository,
    IMenuRepository menuRepository
    )
    : BaseHandler<DeleteMenuItemRequest, DeleteMenuItemResponse>(logger, mapper, restaurantsRepository, menuRepository)
{
    public override async Task<DeleteMenuItemResponse> Handle(DeleteMenuItemRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {@Request}", request);
        logger.LogInformation("Deleting menu item {MenuItemId} from restaurant id {RestaurantId}...", request.MenuItemId, request.RestaurantId);

        var restaurant = await _restaurantsRepository!.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId);

        var menuItem = restaurant.MenuItems.FirstOrDefault(i => i.Id == request.MenuItemId)
            ?? throw new NotFoundException(nameof(MenuItem), request.MenuItemId);

        await _menuRepository!.DeleteMenuItemAsync(menuItem);

        return new DeleteMenuItemResponse
        {
            IsSuccess = true,
            Message = string.Format("Menu item id {0} deleted successfully.", request.MenuItemId)
        };
    }
}
