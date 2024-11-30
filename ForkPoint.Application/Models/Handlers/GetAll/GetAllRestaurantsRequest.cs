using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetAll;
public record GetAllRestaurantsRequest : IRequest<GetAllRestaurantsResponse>;