using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetMenuItems;

public record GetMenuItemsResponse(IEnumerable<MenuItemModel> Menu) : BaseHandlerResponse;