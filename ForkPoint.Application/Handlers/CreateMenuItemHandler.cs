using AutoMapper;
using ForkPoint.Application.Models.Handlers.CreateMenuItem;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class CreateMenuItemHandler(
    ILogger<CreateMenuItemHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository,
    IMenuRepository menuRepository
)
    : IRequestHandler<CreateMenuItemRequest, CreateMenuItemResponse>
{
    public async Task<CreateMenuItemResponse> Handle(
        CreateMenuItemRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Creating new menu item for restaurant id {RestaurantId}...", request.RestaurantId);

        // Check if restaurant exists
        _ = await restaurantsRepository.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        // Map request to MenuItem
        var menuItem = mapper.Map<MenuItem>(request);

        // Create new menu item
        var menuItemId = await menuRepository.CreateMenuItemAsync(menuItem);

        return new CreateMenuItemResponse(menuItemId);
    }
}