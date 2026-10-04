namespace BigLion.CPA.Application.Features.LabItems.Commands.Delete;

public class DeleteLabItemCommandValidator : AbstractValidator<DeleteLabItemCommand>
{
    public DeleteLabItemCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
