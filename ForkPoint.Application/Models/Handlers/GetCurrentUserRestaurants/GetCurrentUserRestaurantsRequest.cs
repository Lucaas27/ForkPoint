using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetCurrentUserRestaurants;

public record GetCurrentUserRestaurantsRequest : IRequest<GetCurrentUserRestaurantsResponse>;