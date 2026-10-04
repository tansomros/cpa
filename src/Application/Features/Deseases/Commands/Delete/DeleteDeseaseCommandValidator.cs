namespace BigLion.CPA.Application.Features.Deseases.Commands.Delete;

public class DeleteDeseaseCommandValidator : AbstractValidator<DeleteDeseaseCommand>
{
    public DeleteDeseaseCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
