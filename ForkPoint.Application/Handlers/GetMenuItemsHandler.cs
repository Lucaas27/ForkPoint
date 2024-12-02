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
    IMenuRepository menuRepository,
    IRestaurantRepository restaurantRepository)
    : BaseHandler<GetMenuItemsRequest, GetMenuItemsResponse>(logger, mapper, restaurantRepository, menuRepository)
{
    public override async Task<GetMenuItemsResponse> Handle(GetMenuItemsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Fetching menu...");

        var restaurant = await _restaurantsRepository!.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(MenuItem), request.RestaurantId);

        var response = new GetMenuItemsResponse
        {
            IsSuccess = true,
            Menu = _mapper.Map<IEnumerable<MenuItemModel>>(restaurant.MenuItems)
        };

        return response;
    }
}
