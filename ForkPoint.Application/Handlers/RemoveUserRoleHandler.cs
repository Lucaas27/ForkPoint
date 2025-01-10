using ForkPoint.Application.Models.Handlers.RemoveUserRole;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class RemoveUserRoleHandler(
    ILogger<RemoveUserRoleHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole<int>> roleManager
) : IRequestHandler<RemoveUserRoleRequest, RemoveUserRoleResponse>
{
    public async Task<RemoveUserRoleResponse> Handle(RemoveUserRoleRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing role {Role} from user with email {Email}", request.Role, request.Email);

        var user = await userManager.FindByEmailAsync(request.Email) ??
                   throw new NotFoundException(nameof(User), request.Email);

        var role = await roleManager.FindByNameAsync(request.Role) ??
                   throw new NotFoundException(nameof(IdentityRole), request.Role);

        var result = await userManager.RemoveFromRoleAsync(user, role.Name!);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to remove role {Role} from user with email {Email}", request.Role, request.Email);
            return new RemoveUserRoleResponse
            {
                IsSuccess = false,
                Message =
                    $"Failed to remove role {request.Role} from user with email {request.Email}. {string.Join(Environment.NewLine, result.Errors.Select(e => e.Description))}"
            };
        }

        logger.LogInformation("Role {Role} removed from user with email {Email}", request.Role, request.Email);

        return new RemoveUserRoleResponse
        {
            IsSuccess = true,
            Message = $"Role {request.Role} removed from user with email {request.Email}"
        };
    }
}