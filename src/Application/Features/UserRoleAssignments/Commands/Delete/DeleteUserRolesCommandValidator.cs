namespace BigLion.CPA.Application.Features.UserRoleAssignments.Commands.Delete;

public class DeleteUserRolesCommandValidator : AbstractValidator<DeleteUserRolesCommand>
{
    public DeleteUserRolesCommandValidator()
    {
        RuleFor(x => x.RoleID).GreaterThan(0);
        RuleFor(x => x.UserID).GreaterThan(0);
    }
}
