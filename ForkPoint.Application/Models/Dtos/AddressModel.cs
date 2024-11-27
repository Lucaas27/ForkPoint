namespace ForkPoint.Application.Models.Dtos;

public record AddressModel
{
    public int Id { get; init; }
    public string Street { get; init; } = default!;
    public string? City { get; init; } = default!;
    public string? County { get; init; }
    public string PostCode { get; init; } = default!;
    public string? Country { get; init; } = default!;

}