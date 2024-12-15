namespace ForkPoint.Application.Models.Handlers.LoginUser;

public record LoginResponse(string AccessToken) : BaseHandlerResponse;