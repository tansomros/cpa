namespace BigLion.CPA.Application.Features.PharmacyTypes.Commands.Delete;

public class DeletePharmacyTypeCommandValidator : AbstractValidator<DeletePharmacyTypeCommand>
{
    public DeletePharmacyTypeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
