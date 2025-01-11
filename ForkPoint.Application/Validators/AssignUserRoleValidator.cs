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
            .Must(x => x.Equals(AppUserRoles.Admin, StringComparison.OrdinalIgnoreCase)
                       || x.Equals(AppUserRoles.Owner, StringComparison.OrdinalIgnoreCase)
                       || x.Equals(AppUserRoles.User, StringComparison.OrdinalIgnoreCase))
            .WithMessage(
                $"Role must be one of {AppUserRoles.Admin}, {AppUserRoles.Owner} or {AppUserRoles.User}");
    }
}