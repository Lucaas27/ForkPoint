namespace ForkPoint.Domain.Entities;

public class Address
{
    public int Id { get; init; }
    public int RestaurantId { get; init; } // Foreign Key
    public Restaurant Restaurant { get; init; } = null!;
    public string Street { get; init; } = null!;
    public string? City { get; init; }
    public string? County { get; init; }
    public string PostCode { get; init; } = null!;
    public string? Country { get; init; }
}