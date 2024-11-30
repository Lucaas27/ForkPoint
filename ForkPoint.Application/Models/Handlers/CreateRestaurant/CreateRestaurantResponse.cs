namespace ForkPoint.Application.Models.Handlers.CreateRestaurant;
public record CreateRestaurantResponse : BaseHandlerResponse
{
    public int NewRecordId { get; init; }
}
