namespace ForkPoint.Domain.Entities;

public class MenuItem
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsVegetarian { get; init; }
    public bool IsVegan { get; init; }
    public int? KiloCalories { get; init; }
    public int RestaurantId { get; init; } // Foreign Key

    // Navigation properties
    public Restaurant Restaurant { get; init; } = default!;
}