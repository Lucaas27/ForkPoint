using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetAll;
public record GetAllRestaurantsResponse : BaseHandlerResponse
{
    public IEnumerable<RestaurantModel> Restaurants { get; init; } = [];
}
