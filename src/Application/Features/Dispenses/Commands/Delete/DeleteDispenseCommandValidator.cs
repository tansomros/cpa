namespace BigLion.CPA.Application.Features.Dispenses.Commands.Delete;

public class DeleteDispenseCommandValidator : AbstractValidator<DeleteDispenseCommand>
{
    public DeleteDispenseCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
