using System.Text.Json.Serialization;

namespace ForkPoint.Application.Models.Dtos;

public record AddressModel
{
    [JsonIgnore] public int Id { get; init; }

    public string Street { get; init; } = null!;
    public string? City { get; init; }
    public string? County { get; init; }
    public string PostCode { get; init; } = null!;
    public string? Country { get; init; }
}