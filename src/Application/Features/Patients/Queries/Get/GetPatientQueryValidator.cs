namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

public class GetPatientQueryValidator : AbstractValidator<GetPatientQuery>
{
    public GetPatientQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
