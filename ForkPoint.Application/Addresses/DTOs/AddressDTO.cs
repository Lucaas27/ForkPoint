using ForkPoint.Domain.Entities;

namespace ForkPoint.Application.Addresses.DTOs;

public record AddressDTO
{
    public int Id { get; init; }
    public string Street { get; init; } = default!;
    public string City { get; init; } = default!;
    public string? County { get; init; }
    public string PostCode { get; init; } = default!;
    public string Country { get; init; } = default!;

    public static explicit operator AddressDTO?(Address address)
    {
        if (address is null) return null;

        return new AddressDTO
        {
            Id = address.Id,
            Street = address.Street,
            City = address.City,
            County = address.County,
            PostCode = address.PostCode,
            Country = address.Country
        };
    }

}