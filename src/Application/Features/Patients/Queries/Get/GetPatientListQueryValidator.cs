namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

public class GetPatientListQueryValidator : AbstractValidator<GetPatientListQuery>
{
    public GetPatientListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Limit).InclusiveBetween(1, 200);
    }
}
