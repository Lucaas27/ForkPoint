using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetMenuItems;

public record GetMenuItemsRequest(int RestaurantId) : IRequest<GetMenuItemsResponse>;