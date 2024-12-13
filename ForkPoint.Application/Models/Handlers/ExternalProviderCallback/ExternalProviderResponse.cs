// Ignore Spelling: auth

namespace ForkPoint.Application.Models.Handlers.ExternalProviderCallback;

public record ExternalProviderResponse : BaseHandlerResponse, IEquatable<BaseHandlerResponse>
{
    public string AccessToken { get; init; } = string.Empty;
}
