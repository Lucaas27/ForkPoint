using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetAllRestaurants;

public record GetAllRestaurantsResponse : BasePaginatedResponse<RestaurantModel>
{
    public GetAllRestaurantsResponse(
        IEnumerable<RestaurantModel> items,
        int totalItemsCount,
        int pageNumber,
        int pageSize
    ) : base(items, totalItemsCount, pageNumber, pageSize)
    {
    }
}
