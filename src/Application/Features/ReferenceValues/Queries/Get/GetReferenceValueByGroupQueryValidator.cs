#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Queries.Get;

public class GetReferenceValueByGroupQueryValidator : AbstractValidator<GetReferenceValueByGroupQuery>
{
    public GetReferenceValueByGroupQueryValidator()
    {
        RuleFor(x => x.ReferenceGroupId).GreaterThan(0);
    }
}
