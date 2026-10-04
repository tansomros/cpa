namespace BigLion.CPA.Application.Features.MTMs.Commands.Delete;

public class DeleteMTMCommandValidator : AbstractValidator<DeleteMTMCommand>
{
    public DeleteMTMCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
