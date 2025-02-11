namespace ForkPoint.Domain.Entities;

public class Address
{
    public int Id { get; init; }
    public int RestaurantId { get; init; } // Foreign Key
    public Restaurant Restaurant { get; init; } = null!;
    public string Street { get; set; } = null!;
    public string? City { get; set; }
    public string? County { get; set; }
    public string PostCode { get; set; } = null!;
    public string? Country { get; set; }
}