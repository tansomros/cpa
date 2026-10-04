namespace BigLion.CPA.Application.Features.Registers.Commands.Delete;

public class DeleteRegisterCommandValidator : AbstractValidator<DeleteRegisterCommand>
{
    public DeleteRegisterCommandValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
