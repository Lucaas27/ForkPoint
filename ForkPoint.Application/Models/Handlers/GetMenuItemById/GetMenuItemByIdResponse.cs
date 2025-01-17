using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetMenuItemById;

public record GetMenuItemByIdResponse(MenuItemModel MenuItem) : BaseResponse;