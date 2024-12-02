using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetMenuItemById;
public record GetMenuItemByIdResponse : BaseHandlerResponse
{
    public MenuItemModel MenuItem { get; init; } = default!;
};
