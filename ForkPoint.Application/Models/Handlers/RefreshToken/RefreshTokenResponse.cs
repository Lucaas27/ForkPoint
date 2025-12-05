namespace ForkPoint.Application.Models.Handlers.RefreshToken;

public record RefreshTokenResponse(string? Token = null, DateTime? Expiry = null) : BaseResponse;