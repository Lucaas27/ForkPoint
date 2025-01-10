using ForkPoint.Application.Models.Handlers.AssignUserRole;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class AssignUserRoleHandler(
    ILogger<AssignUserRoleHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole<int>> roleManager
) : BaseHandler<AssignUserRoleRequest, AssignUserRoleResponse>
{
    public override async Task<AssignUserRoleResponse> Handle(
        AssignUserRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Assigning role {Role} to user with email {Email}", request.Role, request.Email);

        var user = await userManager.FindByEmailAsync(request.Email) ??
                   throw new NotFoundException(nameof(User), request.Email);

        var role = await roleManager.FindByNameAsync(request.Role) ??
                   throw new NotFoundException(nameof(IdentityRole), request.Role);

        var result = await userManager.AddToRoleAsync(user, role.Name!);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to assign role {Role} to user with email {Email}", request.Role, request.Email);
            return new AssignUserRoleResponse
            {
                IsSuccess = false,
                Message =
                    $"Failed to assign role {request.Role} to user with email {request.Email}. {string.Join(Environment.NewLine, result.Errors.Select(e => e.Description))}"
            };
        }

        logger.LogInformation("Role {Role} assigned to user with email {Email}", request.Role, request.Email);

        return new AssignUserRoleResponse
        {
            IsSuccess = true,
            Message = $"Role {request.Role} assigned to user with email {request.Email}"
        };
    }
}