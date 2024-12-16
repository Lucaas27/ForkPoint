using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetMenuItems;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class GetMenuItemsHandler(
    ILogger<GetMenuItemsHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository
)
    : BaseHandler<GetMenuItemsRequest, GetMenuItemsResponse>
{
    public override async Task<GetMenuItemsResponse> Handle(
        GetMenuItemsRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Fetching menu for restaurant {@Restaurant}...", request.RestaurantId);

        var restaurant = await restaurantRepository.GetRestaurantByIdAsync(request.RestaurantId)
                         ?? throw new NotFoundException(nameof(MenuItem), request.RestaurantId);

        var menuItemDto = mapper.Map<IEnumerable<MenuItemModel>>(restaurant.MenuItems);

        var response = new GetMenuItemsResponse(menuItemDto)
        {
            IsSuccess = true
        };

        return response;
    }
}