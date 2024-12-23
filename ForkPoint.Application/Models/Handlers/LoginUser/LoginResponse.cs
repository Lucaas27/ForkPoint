namespace ForkPoint.Application.Models.Handlers.LoginUser;

public record
    LoginResponse(string? AccessToken = null, string? RefreshToken = null, DateTime? Expiry = null)
    : BaseHandlerResponse;