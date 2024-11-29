using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetById;
public record GetByIdResponse : BaseHandlerResponse
{
    public RestaurantDetailsModel? Restaurant { get; init; }
}
