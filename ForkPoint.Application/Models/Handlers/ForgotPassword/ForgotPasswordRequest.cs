using MediatR;

namespace ForkPoint.Application.Models.Handlers.ForgotPassword;

public record ForgotPasswordRequest(string Email) : IRequest<ForgotPasswordResponse>;