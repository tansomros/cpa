namespace BigLion.CPA.Application.Features.BehaviorProblemItems.Commands.Delete;

public class DeleteBehaviorProblemItemCommandValidator : AbstractValidator<DeleteBehaviorProblemItemCommand>
{
    public DeleteBehaviorProblemItemCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
