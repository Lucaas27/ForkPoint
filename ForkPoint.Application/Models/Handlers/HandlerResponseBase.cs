namespace ForkPoint.Application.Models.Handlers;
public record HandlerResponseBase()
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
}
