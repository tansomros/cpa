namespace BigLion.CPA.Application.Features.Hospitals.Queries.Get;

public class GetHospitalQueryValidator : AbstractValidator<GetHospitalQuery>
{
    public GetHospitalQueryValidator()
    {
        RuleFor(x => x.HospitalUID).GreaterThan(0);
    }
}
