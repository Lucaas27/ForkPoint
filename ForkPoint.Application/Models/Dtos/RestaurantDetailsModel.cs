namespace ForkPoint.Application.Models.Dtos;

public record RestaurantDetailsModel
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Category { get; init; } = null!;
    public bool HasDelivery { get; init; } = false;
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }
    public AddressModel Address { get; init; } = null!;
    public ICollection<MenuItemModel> MenuItems { get; init; } = new List<MenuItemModel>();
}