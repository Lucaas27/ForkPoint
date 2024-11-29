using MediatR;
using System.Text.Json.Serialization;

namespace ForkPoint.Application.Models.Handlers.UpdateRestaurant;
public record UpdateRestaurantRequest : IRequest<UpdateRestaurantResponse>
{
    [JsonIgnore]
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public bool HasDelivery { get; init; }
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }


}
