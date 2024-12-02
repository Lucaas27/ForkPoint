using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetMenuItems;

public record GetMenuItemsResponse : BaseHandlerResponse
{
    public IEnumerable<MenuItemModel> Menu { get; init; } = [];
}