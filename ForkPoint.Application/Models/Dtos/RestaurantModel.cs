namespace ForkPoint.Application.Models.Dtos;
public record RestaurantModel
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string Category { get; init; } = default!;
    public bool HasDelivery { get; init; } = default;
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }

}