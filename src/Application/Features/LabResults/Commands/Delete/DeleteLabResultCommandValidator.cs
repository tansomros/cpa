namespace BigLion.CPA.Application.Features.LabResults.Commands.Delete;

public class DeleteLabResultCommandValidator : AbstractValidator<DeleteLabResultCommand>
{
    public DeleteLabResultCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
