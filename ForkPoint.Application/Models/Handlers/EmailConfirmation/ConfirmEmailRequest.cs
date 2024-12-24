using MediatR;

namespace ForkPoint.Application.Models.Handlers.EmailConfirmation;

public record ConfirmEmailRequest(string TokenEmail, string Email) : IRequest<ConfirmEmailResponse>;