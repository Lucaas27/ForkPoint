using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetMenu;
public record GetMenuRequest(int RestaurantId) : IRequest<GetMenuResponse>;
