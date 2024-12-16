using MediatR;

namespace ForkPoint.Application.Models.Handlers.DeleteMenuItem;

public record DeleteMenuItemRequest(int RestaurantId, int MenuItemId) : IRequest<DeleteMenuItemResponse>;