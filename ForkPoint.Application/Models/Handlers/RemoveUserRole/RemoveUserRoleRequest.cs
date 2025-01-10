using MediatR;

namespace ForkPoint.Application.Models.Handlers.RemoveUserRole;

public record RemoveUserRoleRequest(string Email, string Role) : IRequest<RemoveUserRoleResponse>;