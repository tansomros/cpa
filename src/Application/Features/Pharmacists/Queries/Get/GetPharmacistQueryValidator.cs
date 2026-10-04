namespace BigLion.CPA.Application.Features.Pharmacists.Queries.Get;

public class GetPharmacistQueryValidator : AbstractValidator<GetPharmacistQuery>
{
    public GetPharmacistQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
