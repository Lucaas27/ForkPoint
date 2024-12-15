using MediatR;

namespace ForkPoint.Application.Models.Handlers.LoginUser;

public record LoginRequest : IRequest<LoginResponse>;