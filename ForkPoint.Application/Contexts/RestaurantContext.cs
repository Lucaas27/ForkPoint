using Microsoft.AspNetCore.Http;

namespace ForkPoint.Application.Contexts;

public record RestaurantContext(IHttpContextAccessor HttpContextAccessor) : IRestaurantContext
{
    public int GetTargetedRestaurantId()
    {
        var restaurantId = HttpContextAccessor.HttpContext?.Request.RouteValues["restaurantId"]?.ToString();

        if (restaurantId is null)
        {
            throw new InvalidOperationException("Restaurant ID not found in the current context");
        }

        if (!int.TryParse(restaurantId, out var id))
        {
            throw new InvalidCastException("Restaurant ID is not an INT");
        }

        return id;
    }
}