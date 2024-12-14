using MediatR;
using System.Text.Json.Serialization;

namespace ForkPoint.Application.Models.Handlers.CreateMenuItem;
public record CreateMenuItemRequest() : IRequest<CreateMenuItemResponse>
{
    [JsonIgnore]
    public int RestaurantId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; } = 0;
    public string? ImageUrl { get; init; }
    public bool IsVegetarian { get; init; } = false;
    public bool IsVegan { get; init; } = false;
    public int? KiloCalories { get; init; }
}
