using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetAll;
public record GetAllResponse : HandlerResponseBase
{
    public IEnumerable<RestaurantModel> Restaurants { get; init; } = [];
}
