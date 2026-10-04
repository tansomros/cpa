namespace BigLion.CPA.Application.Features.BehaviorProblems.Commands.Delete;

public class DeleteBehaviorProblemCommandValidator : AbstractValidator<DeleteBehaviorProblemCommand>
{
    public DeleteBehaviorProblemCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
