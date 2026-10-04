namespace BigLion.CPA.Application.Features.Patients.Queries.Get;

public class GetPatientByNationIdQueryValidator : AbstractValidator<GetPatientByNationIdQuery>
{
    public GetPatientByNationIdQueryValidator()
    {
        RuleFor(x => x.NationId).NotEmpty();
    }
}
