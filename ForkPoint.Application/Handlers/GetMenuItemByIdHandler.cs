﻿
using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetMenuItemById;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class GetMenuItemByIdHandler(
    ILogger<GetMenuItemByIdHandler> logger,
    IMapper mapper,
    IRestaurantRepository restaurantRepository)
   : BaseHandler<GetMenuItemByIdRequest, GetMenuItemByIdResponse>(logger, mapper, restaurantRepository)
{
    public override async Task<GetMenuItemByIdResponse> Handle(GetMenuItemByIdRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Request: {@Request}", request);
        logger.LogInformation("Fetching menu item {menuId} from restaurant {restaurantId}...", request.MenuItemId, request.RestaurantId);

        var restaurant = await _restaurantsRepository!.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId);

        var menuItem = restaurant.MenuItems.FirstOrDefault(m => m.Id == request.MenuItemId)
            ?? throw new NotFoundException(nameof(MenuItem), request.MenuItemId);

        return new GetMenuItemByIdResponse
        {
            IsSuccess = true,
            MenuItem = _mapper.Map<MenuItemModel>(menuItem)
        };

    }
}
