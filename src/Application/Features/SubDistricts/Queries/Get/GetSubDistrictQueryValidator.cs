namespace BigLion.CPA.Application.Features.SubDistricts.Queries.Get;

public class GetSubDistrictQueryValidator : AbstractValidator<GetSubDistrictQuery>
{
    public GetSubDistrictQueryValidator()
    {
        RuleFor(x => x.SubDistrictId).NotEmpty();
    }
}
