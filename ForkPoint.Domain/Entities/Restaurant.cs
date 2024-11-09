namespace ForkPoint.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; } = default;
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }

    // Navigation properties
    public Address Address { get; set; } = new Address();
    public ICollection<MenuItem> MenuItems { get; set; } = [];
}
