namespace ForkPoint.Application.Models.Dtos;

public record MenuItemModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; } = 0;
    public string? ImageUrl { get; set; }
    public bool IsVegetarian { get; set; } = false;
    public bool IsVegan { get; set; } = false;
    public int? KiloCalories { get; set; }
}