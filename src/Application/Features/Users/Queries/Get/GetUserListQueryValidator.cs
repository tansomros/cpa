namespace BigLion.CPA.Application.Features.Users.Queries.Get;

public class GetUserListQueryValidator : AbstractValidator<GetUserListQuery>
{
    public GetUserListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}
