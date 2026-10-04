namespace BigLion.CPA.Application.Features.MTMDrugProblems.Commands.Delete;

public class DeleteMTMDrugProblemCommandValidator : AbstractValidator<DeleteMTMDrugProblemCommand>
{
    public DeleteMTMDrugProblemCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
