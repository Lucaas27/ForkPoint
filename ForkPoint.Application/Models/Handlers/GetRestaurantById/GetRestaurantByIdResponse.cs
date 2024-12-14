using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetRestaurantById;

public record GetRestaurantByIdResponse(RestaurantDetailsModel Restaurant) : BaseHandlerResponse;