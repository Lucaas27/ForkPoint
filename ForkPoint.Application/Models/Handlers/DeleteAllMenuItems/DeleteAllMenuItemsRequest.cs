using MediatR;

namespace ForkPoint.Application.Models.Handlers.DeleteAllMenuItems;

public record DeleteAllMenuItemsRequest(int RestaurantId) : IRequest<DeleteAllMenuItemsResponse>;