namespace BigLion.CPA.Application.Features.ProvinceGroups.Queries.Get;

public class GetProvinceGroupQueryValidator : AbstractValidator<GetProvinceGroupQuery>
{
    public GetProvinceGroupQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
