using AutoMapper;
using ForkPoint.Application.Contexts;
using ForkPoint.Application.Models.Handlers.AdminUpdateUserDetails;
using ForkPoint.Domain.Entities;
using ForkPoint.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForkPoint.Application.Handlers;

public class AdminUpdateUserDetailsHandler(
    ILogger<AdminUpdateUserDetailsHandler> logger,
    IMapper mapper,
    IUserContext userContext,
    UserManager<User> userManager
) : BaseHandler<AdminUpdateUserDetailsRequest, AdminUpdateUserDetailsResponse>
{
    public override async Task<AdminUpdateUserDetailsResponse> Handle(
        AdminUpdateUserDetailsRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = userContext.GetCurrentUser() ?? throw new InvalidOperationException("User not authenticated");

        logger.LogInformation("Admin updating user details with {Request} for user {Email}", request, user.Email);

        var targetUserId = userContext.GetTargetUserId();

        if (targetUserId <= 0)
        {
            return new AdminUpdateUserDetailsResponse
            {
                IsSuccess = false,
                Message = "Target User ID is not valid. Value must be greater than 0."
            };
        }

        var dbUser = await userManager.FindByIdAsync(targetUserId.ToString()) ??
                     throw new NotFoundException(nameof(User), targetUserId.ToString());

        // Map the request data to the domain model
        mapper.Map(request, dbUser);

        var result = await userManager.UpdateAsync(dbUser);

        if (!result.Succeeded)
        {
            throw new Exception("Failed to update user details");
        }

        return new AdminUpdateUserDetailsResponse
        {
            IsSuccess = true,
            Message = "User details updated successfully"
        };
    }
}