namespace BigLion.CPA.Application.Features.DrugProblemItems.Commands.Delete;

public class DeleteDrugProblemItemCommandValidator : AbstractValidator<DeleteDrugProblemItemCommand>
{
    public DeleteDrugProblemItemCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
