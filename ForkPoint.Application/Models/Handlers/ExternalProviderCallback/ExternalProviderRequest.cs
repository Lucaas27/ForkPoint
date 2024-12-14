using MediatR;
using Microsoft.AspNetCore.Http;

namespace ForkPoint.Application.Models.Handlers.ExternalProviderCallback;

public record ExternalProviderRequest()
    : IRequest<ExternalProviderResponse>;