
namespace ForkPoint.Domain.Entities;

public class Restaurant
{

    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string Category { get; init; } = null!;
    public bool HasDelivery { get; init; }
    public string? Email { get; init; }
    public string? ContactNumber { get; init; }

    // Navigation properties
    public Address Address { get; init; } = null!;
    public ICollection<MenuItem> MenuItems { get; init; } = null!;
}
