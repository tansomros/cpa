namespace BigLion.CPA.Application.Features.ServiceRecords.Queries.Get;

public class GetServicesQueryValidator : AbstractValidator<GetServicesQuery>
{
    public GetServicesQueryValidator()
    {
        RuleFor(x => x.itemID).GreaterThan(0);
    }
}
