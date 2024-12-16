using ForkPoint.Application.Models.Handlers;
using MediatR;

namespace ForkPoint.Application.Handlers;

/// <summary>
///     BaseHandler is an abstract class that implements the IRequestHandler interface.
///     It provides common functionality for handling requests in the application.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseHandlerResponse
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}