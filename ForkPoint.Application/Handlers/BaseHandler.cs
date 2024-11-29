using AutoMapper;
using ForkPoint.Application.Models.Handlers;
using ForkPoint.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;
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
