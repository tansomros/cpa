namespace BigLion.CPA.Application.Features.Roles.Queries.Get;

public class GetRoleQueryValidator : AbstractValidator<GetRoleQuery>
{
    public GetRoleQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
