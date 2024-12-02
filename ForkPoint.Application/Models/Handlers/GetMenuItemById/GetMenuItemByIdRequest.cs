using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetMenuItemById;
public record GetMenuItemByIdRequest(int RestaurantId, int MenuItemId) : IRequest<GetMenuItemByIdResponse>;
