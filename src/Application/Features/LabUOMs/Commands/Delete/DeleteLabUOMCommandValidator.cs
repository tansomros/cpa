namespace BigLion.CPA.Application.Features.LabUOMs.Commands.Delete;

public class DeleteLabUOMCommandValidator : AbstractValidator<DeleteLabUOMCommand>
{
    public DeleteLabUOMCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
