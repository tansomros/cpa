namespace BigLion.CPA.Application.Features.MTMDrugRemains.Commands.Delete;

public class DeleteMTMDrugRemainCommandValidator : AbstractValidator<DeleteMTMDrugRemainCommand>
{
    public DeleteMTMDrugRemainCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
