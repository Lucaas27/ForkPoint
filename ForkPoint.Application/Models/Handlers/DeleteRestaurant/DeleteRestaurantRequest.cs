using MediatR;

namespace ForkPoint.Application.Models.Handlers.DeleteRestaurant;
public record DeleteRestaurantRequest(int Id) : IRequest<DeleteRestaurantResponse>;
