using MediatR;
using Microsoft.AspNetCore.Http;

namespace ForkPoint.Application.Models.Handlers.ExternalProviderCallback;

public record ExternalProviderRequest(HttpContext HttpCxt, string AuthenticationScheme)
    : IRequest<ExternalProviderResponse>;