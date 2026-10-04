#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Queries.Get;

public class GetReferenceValuePaginatedListQueryValidator : AbstractValidator<GetReferenceValuePaginatedListQuery>
{
    public GetReferenceValuePaginatedListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Length).InclusiveBetween(1, 200);
    }
}
