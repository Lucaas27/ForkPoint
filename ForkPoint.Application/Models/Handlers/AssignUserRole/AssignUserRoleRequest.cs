using MediatR;

namespace ForkPoint.Application.Models.Handlers.AssignUserRole;

public record AssignUserRoleRequest(string Email, string Role) : IRequest<AssignUserRoleResponse>;