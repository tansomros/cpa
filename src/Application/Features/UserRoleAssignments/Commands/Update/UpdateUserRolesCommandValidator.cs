namespace BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Update;

public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
{
    public UpdateUserRolesCommandValidator()
    {
        RuleFor(x => x.RoleID).GreaterThan(0);
        RuleFor(x => x.UserID).GreaterThan(0);
    }
}
