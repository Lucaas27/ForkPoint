using AutoMapper;
using ForkPoint.Application.Models.Handlers;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

/// <summary>
/// BaseHandler is an abstract class that implements the IRequestHandler interface.
/// It provides common functionality for handling requests in the application.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="logger">The logger instance for logging information.</param>
/// <param name="mapper">The AutoMapper instance for object mapping.</param>
/// <param name="restaurantsRepository">The repository instance for accessing restaurant data.</param>
public abstract class BaseHandler<TRequest, TResponse>(
    ILogger<BaseHandler<TRequest, TResponse>> logger,
    IMapper mapper,
    IRestaurantRepository restaurantsRepository)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseHandlerResponse
{
    protected readonly ILogger<BaseHandler<TRequest, TResponse>> _logger = logger;
    protected readonly IMapper _mapper = mapper;
    protected readonly IRestaurantRepository _restaurantsRepository = restaurantsRepository;

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
