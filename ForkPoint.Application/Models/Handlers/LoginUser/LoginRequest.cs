using MediatR;

namespace ForkPoint.Application.Models.Handlers.LoginUser;

public record LoginRequest(string Email, string Password) : IRequest<LoginResponse>;