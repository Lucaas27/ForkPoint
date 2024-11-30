using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetMenu;

public record GetMenuResponse : BaseHandlerResponse
{
    public IEnumerable<MenuItemModel> Menu { get; init; } = [];
}