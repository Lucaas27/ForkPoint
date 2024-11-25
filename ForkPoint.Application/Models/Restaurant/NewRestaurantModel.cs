namespace ForkPoint.Application.Models.Restaurant;
public record NewRestaurantModel
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string Category { get; init; } = default!;
    public bool HasDelivery { get; init; } = default;

    public string? Email { get; init; }
    public string? ContactNumber { get; init; }


    public string Street { get; init; } = default!;
    public string? City { get; init; } = default!;
    public string? County { get; init; }
    public string PostCode { get; init; } = default!;
    public string? Country { get; init; } = default!;
}
