namespace BigLion.CPA.Application.Features.Prefixs.Queries.Get;

public class GetPrefixQueryValidator : AbstractValidator<GetPrefixQuery>
{
    public GetPrefixQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
