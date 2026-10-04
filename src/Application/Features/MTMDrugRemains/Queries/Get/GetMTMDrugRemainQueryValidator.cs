namespace BigLion.CPA.Application.Features.MTMDrugRemains.Queries.Get;

public class GetMTMDrugRemainQueryValidator : AbstractValidator<GetMTMDrugRemainQuery>
{
    public GetMTMDrugRemainQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
