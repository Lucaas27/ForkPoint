using ForkPoint.Application.Addresses.DTOs;
using ForkPoint.Application.MenuItems.DTOs;
using ForkPoint.Domain.Entities;

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


    public static explicit operator RestaurantDTO?(Restaurant restaurant)
    {
        if (restaurant is null) return null;

        return new RestaurantDTO
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            Category = restaurant.Category,
            HasDelivery = restaurant.HasDelivery,
            Email = restaurant.Email,
            ContactNumber = restaurant.ContactNumber,
            Address = restaurant.Address is not null ? (AddressDTO)restaurant.Address! : new AddressDTO(),
            MenuItems = restaurant.MenuItems?.Where(mi => mi is not null).Select(mi => (MenuItemDTO)mi!).ToList() ?? new List<MenuItemDTO>()
        };
    }
}