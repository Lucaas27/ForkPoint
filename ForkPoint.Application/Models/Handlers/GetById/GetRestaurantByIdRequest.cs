using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetById;
public record GetRestaurantByIdRequest(int Id) : IRequest<GetRestaurantByIdResponse>;