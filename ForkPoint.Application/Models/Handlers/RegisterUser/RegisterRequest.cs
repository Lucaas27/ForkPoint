using MediatR;

namespace ForkPoint.Application.Models.Handlers.RegisterUser;

public record RegisterRequest(string Email, string Password) : IRequest<RegisterResponse>;