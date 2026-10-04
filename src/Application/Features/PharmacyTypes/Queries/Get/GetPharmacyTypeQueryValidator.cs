namespace BigLion.CPA.Application.Features.PharmacyTypes.Queries.Get;

public class GetPharmacyTypeQueryValidator : AbstractValidator<GetPharmacyTypeQuery>
{
    public GetPharmacyTypeQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
