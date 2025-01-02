using MediatR;

namespace ForkPoint.Application.Models.Handlers.EmailConfirmation;

public record ConfirmEmailRequest(string Token, string Email) : IRequest<ConfirmEmailResponse>;