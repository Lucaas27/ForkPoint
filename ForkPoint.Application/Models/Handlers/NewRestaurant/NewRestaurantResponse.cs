namespace ForkPoint.Application.Models.Handlers.NewRestaurant;
public record NewRestaurantResponse : HandlerResponseBase
{
    public int NewRecordId { get; init; }
}
