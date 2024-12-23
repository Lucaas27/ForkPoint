using MediatR;

namespace ForkPoint.Application.Models.Handlers.RefreshToken;

public record RefreshTokenRequest(string AccessToken, string RefreshToken) : IRequest<RefreshTokenResponse>;