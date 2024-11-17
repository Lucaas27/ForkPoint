using ForkPoint.Application.Addresses.DTOs;
using ForkPoint.Application.MenuItems.DTOs;

namespace ForkPoint.Application.Restaurants.DTOs;
public record RestaurantDTO
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string Category { get; init; } = default!;
    public bool HasDelivery { get; init; } = default;
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }
    public AddressDTO Address { get; init; } = default!;
    public ICollection<MenuItemDTO> MenuItems { get; init; } = new List<MenuItemDTO>();

}