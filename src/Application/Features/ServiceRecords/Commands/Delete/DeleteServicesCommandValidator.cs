namespace BigLion.CPA.Application.Features.ServiceRecords.Commands.Delete;

public class DeleteServicesCommandValidator : AbstractValidator<DeleteServicesCommand>
{
    public DeleteServicesCommandValidator()
    {
        RuleFor(x => x.itemID).GreaterThan(0);
    }
}
