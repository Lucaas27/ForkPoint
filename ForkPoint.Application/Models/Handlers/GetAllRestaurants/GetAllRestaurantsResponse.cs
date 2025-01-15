using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;

public record GetAllRestaurantsResponse : BasePaginatedResponse<RestaurantModel>
{
    public GetAllRestaurantsResponse(
        IEnumerable<RestaurantModel> restaurants,
        int totalCount,
        int pageSize,
        int pageNumber
    ) : base(restaurants, totalCount, pageSize, pageNumber)
    {
    }
}