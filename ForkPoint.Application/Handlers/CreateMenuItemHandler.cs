using AutoMapper;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class CreateMenuItemHandler(
    ILogger<CreateMenuItemHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository,
    IMenuRepository menuRepository)
    : BaseHandler<CreateMenuItemRequest, CreateMenuItemResponse>
{
    public override async Task<CreateMenuItemResponse> Handle(CreateMenuItemRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {@Request}", request);
        logger.LogInformation("Creating new menu item for restaurant id {restaurantId}...", request.RestaurantId);

        // Check if restaurant exists
        _ = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId);

        // Map request to MenuItem
        var menuItem = mapper.Map<MenuItem>(request);

        // Create new menu item
        var menuItemId = await menuRepository.CreateMenuItemAsync(menuItem);

        return new CreateMenuItemResponse(menuItemId);
    }
}