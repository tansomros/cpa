namespace BigLion.CPA.Application.Features.UserRoleAssignments.Queries.Get;

public class GetUserRolesQueryValidator : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesQueryValidator()
    {
        RuleFor(x => x.RoleID).GreaterThan(0);
        RuleFor(x => x.UserID).GreaterThan(0);
    }
}
