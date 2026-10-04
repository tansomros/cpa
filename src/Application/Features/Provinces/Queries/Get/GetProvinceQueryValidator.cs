namespace BigLion.CPA.Application.Features.Provinces.Queries.Get;

public class GetProvinceQueryValidator : AbstractValidator<GetProvinceQuery>
{
    public GetProvinceQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
