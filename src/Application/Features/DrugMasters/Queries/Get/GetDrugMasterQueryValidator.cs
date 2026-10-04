namespace BigLion.CPA.Application.Features.DrugMasters.Queries.Get;

public class GetDrugMasterQueryValidator : AbstractValidator<GetDrugMasterQuery>
{
    public GetDrugMasterQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
