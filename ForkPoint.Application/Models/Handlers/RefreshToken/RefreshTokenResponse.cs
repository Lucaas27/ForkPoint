namespace ForkPoint.Application.Models.Handlers.RefreshToken;

public record RefreshTokenResponse(string? Token = null, string? RefreshToken = null, DateTime Expiry = default)
    : BaseHandlerResponse;