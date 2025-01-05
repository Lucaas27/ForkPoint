using MediatR;

namespace ForkPoint.Application.Models.Handlers.Logout;

public record LogoutRequest(string Email) : IRequest<LogoutResponse>;