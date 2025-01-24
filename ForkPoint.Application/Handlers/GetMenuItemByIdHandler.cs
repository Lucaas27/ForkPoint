using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetMenuItemById;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class GetMenuItemByIdHandler(
    ILogger<GetMenuItemByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository
)
    : IRequestHandler<GetMenuItemByIdRequest, GetMenuItemByIdResponse>
{
    public async Task<GetMenuItemByIdResponse> Handle(
        GetMenuItemByIdRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Fetching menu item {MenuId} from restaurant {RestaurantId}...", request.MenuItemId,
            request.RestaurantId);

        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId)
                         ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var menuItem = restaurant.MenuItems.FirstOrDefault(m => m.Id == request.MenuItemId)
                       ?? throw new NotFoundException(nameof(MenuItem), request.MenuItemId.ToString());

        var menuItemDto = mapper.Map<MenuItemModel>(menuItem);

        return new GetMenuItemByIdResponse(menuItemDto)
        {
            IsSuccess = true
        };
    }
}