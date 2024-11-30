using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;
public record GetAllRestaurantsResponse : BaseHandlerResponse
{
    public IEnumerable<RestaurantModel> Restaurants { get; init; } = [];
}
