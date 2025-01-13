using MediatR;

namespace ForkPoint.Application.Models.Handlers.ResendEmailConfirmation;

public record ResendEmailConfirmationRequest(string Email) : IRequest<ResendEmailConfirmationResponse>;