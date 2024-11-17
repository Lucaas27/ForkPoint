using ForkPoint.Domain.Entities;

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

    public static explicit operator MenuItemDTO?(MenuItem menuItem)
    {
        if (menuItem is null) return null;

        return new MenuItemDTO
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            ImageUrl = menuItem.ImageUrl,
            IsVegetarian = menuItem.IsVegetarian,
            IsVegan = menuItem.IsVegan,
            KiloCalories = menuItem.KiloCalories
        };
    }

}