namespace BigLion.CPA.Application.Features.DrugMasters.Commands.Delete;

public class DeleteDrugMasterCommandValidator : AbstractValidator<DeleteDrugMasterCommand>
{
    public DeleteDrugMasterCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
