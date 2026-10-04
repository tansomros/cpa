namespace BigLion.CPA.Application.Features.DrugProblemGroups.Commands.Delete;

public class DeleteDrugProblemGroupCommandValidator : AbstractValidator<DeleteDrugProblemGroupCommand>
{
    public DeleteDrugProblemGroupCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
