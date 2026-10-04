namespace BigLion.CPA.Application.Features.Pharmacists.Commands.Delete;

public class DeletePharmacistCommandValidator : AbstractValidator<DeletePharmacistCommand>
{
    public DeletePharmacistCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
