#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.Queries.Get;

public class GetReferenceValueQueryValidator : AbstractValidator<GetReferenceValueQuery>
{
    public GetReferenceValueQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
