using MediatR;

namespace ForkPoint.Application.Models.Handlers.GetCurrentUser;

public record GetCurrentUserRequest : IRequest<GetCurrentUserResponse>;
