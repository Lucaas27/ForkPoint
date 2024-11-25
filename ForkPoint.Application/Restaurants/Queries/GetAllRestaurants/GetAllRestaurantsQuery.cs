using ForkPoint.Application.Models.Restaurant;
using MediatR;

namespace ForkPoint.Application.Restaurants.Queries.GetAllRestaurants;
public record GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantModel>>
{
}
