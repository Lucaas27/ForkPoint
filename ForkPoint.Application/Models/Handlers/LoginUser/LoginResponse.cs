namespace ForkPoint.Application.Models.Handlers.LoginUser;

public record LoginResponse(string? AccessToken = null, DateTime? Expiry = null) : BaseResponse;