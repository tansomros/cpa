#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.Queries.Get;

public class GetReferenceGroupQueryValidator : AbstractValidator<GetReferenceGroupQuery>
{
    public GetReferenceGroupQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
