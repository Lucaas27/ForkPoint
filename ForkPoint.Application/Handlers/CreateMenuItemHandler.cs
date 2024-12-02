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
    : BaseHandler<CreateMenuItemRequest, CreateMenuItemResponse>(logger, mapper, restaurantsRepository)
{
    public override async Task<CreateMenuItemResponse> Handle(CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Creating new menu item for restaurant id {restaurantId}...", request.RestaurantId);

        var restaurant = await _restaurantsRepository!.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId);

        var menuItem = _mapper.Map<MenuItem>(request);

        var menuItemId = await menuRepository.CreateMenuItemAsync(menuItem);

        return new CreateMenuItemResponse(menuItem.Id);
    }
}
