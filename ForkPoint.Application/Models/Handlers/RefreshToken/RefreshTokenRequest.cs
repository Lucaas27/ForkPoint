using MediatR;

namespace ForkPoint.Application.Models.Handlers.RefreshToken;

public record RefreshTokenRequest(string AccessToken) : IRequest<RefreshTokenResponse>;