using MediatR;
using System.Text.Json.Serialization;

namespace ForkPoint.Application.Models.Handlers.CreateMenuItem;
public record CreateMenuItemRequest() : IRequest<CreateMenuItemResponse>
{
    [JsonIgnore]
    public int RestaurantId { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public decimal Price { get; init; } = default;
    public string? ImageUrl { get; init; }
    public bool IsVegetarian { get; init; } = false;
    public bool IsVegan { get; init; } = false;
    public int? KiloCalories { get; init; }
}
