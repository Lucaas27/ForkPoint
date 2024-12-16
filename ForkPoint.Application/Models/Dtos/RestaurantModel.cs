namespace ForkPoint.Application.Models.Dtos;

public record RestaurantModel
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Category { get; init; } = null!;
}