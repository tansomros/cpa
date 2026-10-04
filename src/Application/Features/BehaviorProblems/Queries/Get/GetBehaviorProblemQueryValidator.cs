namespace BigLion.CPA.Application.Features.BehaviorProblems.Queries.Get;

public class GetBehaviorProblemQueryValidator : AbstractValidator<GetBehaviorProblemQuery>
{
    public GetBehaviorProblemQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
