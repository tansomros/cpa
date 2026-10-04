namespace BigLion.CPA.Application.Features.MTMDeseases.Commands.Delete;

public class DeleteMTMDeseaseCommandValidator : AbstractValidator<DeleteMTMDeseaseCommand>
{
    public DeleteMTMDeseaseCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
