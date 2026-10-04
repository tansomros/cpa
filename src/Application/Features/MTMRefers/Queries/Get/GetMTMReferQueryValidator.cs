namespace BigLion.CPA.Application.Features.MTMRefers.Queries.Get;

public class GetMTMReferQueryValidator : AbstractValidator<GetMTMReferQuery>
{
    public GetMTMReferQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
