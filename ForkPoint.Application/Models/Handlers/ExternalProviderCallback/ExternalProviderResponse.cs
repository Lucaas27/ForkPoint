// Ignore Spelling: auth

namespace ForkPoint.Application.Models.Handlers.ExternalProviderCallback;

public record ExternalProviderResponse(string AccessToken, string RefreshToken, DateTime Expiry) : BaseHandlerResponse;