using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;

public record GetAllRestaurantsResponse(IEnumerable<RestaurantModel> Restaurants) : BaseHandlerResponse;