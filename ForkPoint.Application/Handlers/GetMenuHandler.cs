using AutoMapper;
using ForkPoint.Application.Models.Dtos;
using ForkPoint.Application.Models.Handlers.GetMenu;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using ForkPoint.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
public class GetMenuHandler(
    ILogger<GetMenuHandler> logger,
    IMapper mapper,
    IMenuRepository menuRepository,
    IRestaurantRepository restaurantRepository)
    : BaseHandler<GetMenuRequest, GetMenuResponse>(logger, mapper, restaurantRepository, menuRepository)
{
    public override async Task<GetMenuResponse> Handle(GetMenuRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {@Request}", request);
        _logger.LogInformation("Fetching menu...");

        var restaurant = await _restaurantsRepository!.GetRestaurantByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(MenuItem), request.RestaurantId);

        var menu = await _menuRepository!.GetMenuAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(MenuItem), request.RestaurantId);

        var response = new GetMenuResponse
        {
            IsSuccess = menu.Any(),
            Menu = _mapper.Map<IEnumerable<MenuItemModel>>(menu)
        };

        return response;
    }
}
