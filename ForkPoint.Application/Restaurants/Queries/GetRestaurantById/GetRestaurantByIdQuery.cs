using ForkPoint.Application.Models.Restaurant;
using MediatR;

namespace ForkPoint.Application.Restaurants.Queries.GetRestaurantById;
public record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDetailsModel?>
{
    public int Id { get; } = Id;
}
