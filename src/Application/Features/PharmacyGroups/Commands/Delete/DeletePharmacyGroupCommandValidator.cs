namespace BigLion.CPA.Application.Features.PharmacyGroups.Commands.Delete;

public class DeletePharmacyGroupCommandValidator : AbstractValidator<DeletePharmacyGroupCommand>
{
    public DeletePharmacyGroupCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
