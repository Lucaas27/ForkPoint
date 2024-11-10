namespace ForkPoint.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = default;
    public string? ImageUrl { get; set; }
    public bool IsVegetarian { get; set; } = default;
    public bool IsVegan { get; set; } = default;
    public int RestaurantId { get; set; } // Foreign Key

    // Navigation properties
    public Restaurant? Restaurant { get; set; }
}