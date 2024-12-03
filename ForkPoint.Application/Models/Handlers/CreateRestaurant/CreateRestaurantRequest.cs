﻿using ForkPoint.Application.Models.Dtos;
using MediatR;

namespace ForkPoint.Application.Models.Handlers.CreateRestaurant;
public record CreateRestaurantRequest : IRequest<CreateRestaurantResponse>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public string Category { get; init; } = default!;
    public bool HasDelivery { get; init; } = default;

    public string? Email { get; init; }
    public string? ContactNumber { get; init; }
    public AddressModel? Address { get; init; }

}
