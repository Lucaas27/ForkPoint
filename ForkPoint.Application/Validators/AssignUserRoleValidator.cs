using FluentValidation;
using ForkPoint.Application.Models.Handlers.AssignUserRole;
using ForkPoint.Domain.Constants;

namespace ForkPoint.Application.Validators;

public class AssignUserRoleValidator : AbstractValidator<AssignUserRoleRequest>
{
    public AssignUserRoleValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(x => x.Equals(AppPolicies.AdminPolicy, StringComparison.OrdinalIgnoreCase)
                       || x.Equals(AppPolicies.OwnerPolicy, StringComparison.OrdinalIgnoreCase)
                       || x.Equals(AppPolicies.UserPolicy, StringComparison.OrdinalIgnoreCase))
            .WithMessage(
                $"Role must be one of {AppPolicies.AdminPolicy}, {AppPolicies.OwnerPolicy}, {AppPolicies.UserPolicy}");
    }
}