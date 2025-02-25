﻿using ForkPoint.Application.Models.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace ForkPoint.Application.Models.Handlers.UpdateRestaurant;

public record UpdateRestaurantRequest : IRequest<UpdateRestaurantResponse>
{
    [JsonIgnore] public int Id { get; init; }

    public string? Name { get; init; } = null!;
    public string? Description { get; init; } = null!;
    public bool? HasDelivery { get; init; }
    public string? Email { get; init; } = null!;
    public string? ContactNumber { get; init; } = null!;
    public AddressModel? Address { get; init; } = null!;
}