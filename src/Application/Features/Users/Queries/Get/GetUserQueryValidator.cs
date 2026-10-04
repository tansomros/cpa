namespace BigLion.CPA.Application.Features.Users.Queries.Get;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
