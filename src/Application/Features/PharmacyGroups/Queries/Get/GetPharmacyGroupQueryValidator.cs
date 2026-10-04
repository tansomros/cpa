namespace BigLion.CPA.Application.Features.PharmacyGroups.Queries.Get;

public class GetPharmacyGroupQueryValidator : AbstractValidator<GetPharmacyGroupQuery>
{
    public GetPharmacyGroupQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
