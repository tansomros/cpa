#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Queries.Get;

public class GetReferenceGroupPaginatedListQueryValidator : AbstractValidator<GetReferenceGroupPaginatedListQuery>
{
    public GetReferenceGroupPaginatedListQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Length).InclusiveBetween(1, 200);
    }
}
