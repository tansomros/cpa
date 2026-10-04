namespace BigLion.CPA.Application.Features.MTMBehaviors.Commands.Delete;

public class DeleteMTMBehaviorCommandValidator : AbstractValidator<DeleteMTMBehaviorCommand>
{
    public DeleteMTMBehaviorCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
