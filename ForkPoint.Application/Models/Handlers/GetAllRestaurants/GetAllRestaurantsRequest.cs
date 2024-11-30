using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;
public record GetAllRestaurantsRequest : IRequest<GetAllRestaurantsResponse>;