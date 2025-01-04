using MediatR;

namespace ForkPoint.Application.Models.Handlers.ResetPassword;

public record ResetPasswordRequest(string Password, string ConfirmPassword, string Email, string Token)
    : IRequest<ResetPasswordResponse>;