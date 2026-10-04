namespace BigLion.CPA.Application.Features.Dispenses.Queries.Get;

public class GetDispenseQueryValidator : AbstractValidator<GetDispenseQuery>
{
    public GetDispenseQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
