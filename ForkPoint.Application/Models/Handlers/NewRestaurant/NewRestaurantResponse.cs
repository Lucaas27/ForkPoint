namespace ForkPoint.Application.Models.Handlers.NewRestaurant;
public record NewRestaurantResponse : BaseHandlerResponse
{
    public int NewRecordId { get; init; }
}
