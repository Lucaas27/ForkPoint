namespace ForkPoint.Application.Models.Handlers;
public abstract record BaseHandlerResponse()
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
}
