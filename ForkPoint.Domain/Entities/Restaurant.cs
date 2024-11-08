namespace ForkPoint.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; } = default;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; } = default;

    public string? Email { get; set; } = string.Empty;
    public string? ContactNumber { get; set; } = string.Empty;
    public Address? Address { get; set; }

    public List<MenuItem> MenuItems { get; set; } = [];


}
