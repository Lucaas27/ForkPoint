using ForkPoint.Application.Models.Dtos;

namespace ForkPoint.Application.Models.Handlers.GetExternalRestaurants;

public record GetExternalRestaurantsResponse : BasePaginatedResponse<RestaurantModel>
{
    public GetExternalRestaurantsResponse(
        IEnumerable<RestaurantModel> items,
        int totalItemsCount,
        int pageNumber,
        int pageSize
    ) : base(items, totalItemsCount, pageNumber, pageSize)
    {
    }
}
