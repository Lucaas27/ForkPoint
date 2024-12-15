using MediatR;

namespace ForkPoint.Application.Models.Handlers.ExternalProviderCallback;

public record ExternalProviderRequest
    : IRequest<ExternalProviderResponse>;