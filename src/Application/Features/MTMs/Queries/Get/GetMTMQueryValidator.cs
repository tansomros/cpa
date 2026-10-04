namespace BigLion.CPA.Application.Features.MTMs.Queries.Get;

public class GetMTMQueryValidator : AbstractValidator<GetMTMQuery>
{
    public GetMTMQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
