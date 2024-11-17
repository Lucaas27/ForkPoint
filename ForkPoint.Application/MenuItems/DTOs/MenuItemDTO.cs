namespace ForkPoint.Application.MenuItems.DTOs;

public record MenuItemDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; } = default;
    public string? ImageUrl { get; set; }
    public bool IsVegetarian { get; set; } = default;
    public bool IsVegan { get; set; } = default;
    public int? KiloCalories { get; set; }


}