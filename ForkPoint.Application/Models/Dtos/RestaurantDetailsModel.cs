namespace ForkPoint.Application.Models.Dtos;

public record RestaurantDetailsModel
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string Category { get; init; } = default!;
    public bool HasDelivery { get; init; } = default;
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }
    public AddressModel Address { get; init; } = default!;
    public ICollection<MenuItemModel> MenuItems { get; init; } = new List<MenuItemModel>();
}