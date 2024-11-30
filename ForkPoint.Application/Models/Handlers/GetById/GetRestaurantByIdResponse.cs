using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetById;
public record GetRestaurantByIdResponse : BaseHandlerResponse
{
    public RestaurantDetailsModel? Restaurant { get; init; }
}
