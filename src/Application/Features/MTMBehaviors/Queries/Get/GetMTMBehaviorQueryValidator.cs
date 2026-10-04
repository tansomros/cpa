namespace BigLion.CPA.Application.Features.MTMBehaviors.Queries.Get;

public class GetMTMBehaviorQueryValidator : AbstractValidator<GetMTMBehaviorQuery>
{
    public GetMTMBehaviorQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
