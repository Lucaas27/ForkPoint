using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetRestaurantById;

public record GetRestaurantByIdRequest(int Id) : IRequest<GetRestaurantByIdResponse>;