namespace BigLion.CPA.Application.Features.MTMRefers.Commands.Delete;

public class DeleteMTMReferCommandValidator : AbstractValidator<DeleteMTMReferCommand>
{
    public DeleteMTMReferCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
