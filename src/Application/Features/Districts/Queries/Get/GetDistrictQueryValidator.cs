namespace BigLion.CPA.Application.Features.Districts.Queries.Get;

public class GetDistrictQueryValidator : AbstractValidator<GetDistrictQuery>
{
    public GetDistrictQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
