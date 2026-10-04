namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Queries.Get;

public class GetBehaviorProblemItemQueryValidator : AbstractValidator<GetBehaviorProblemItemQuery>
{
    public GetBehaviorProblemItemQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
