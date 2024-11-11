namespace ForkPoint.Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public int RestaurantId { get; set; } // Foreign Key
    public Restaurant Restaurant { get; set; } = default!;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? County { get; set; }
    public string PostCode { get; set; } = string.Empty;
    public string? Country { get; set; }
}