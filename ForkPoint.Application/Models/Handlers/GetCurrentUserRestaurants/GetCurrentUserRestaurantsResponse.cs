using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetCurrentUserRestaurants;

public record GetCurrentUserRestaurantsResponse(IEnumerable<RestaurantModel> OwnedRestaurants, int UserId)
    : BaseHandlerResponse;