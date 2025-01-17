namespace ForkPoint.Application.Models.Handlers;

/// <summary>
///     Represents the base response for a handler operation.
/// </summary>
public abstract record BaseResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
}