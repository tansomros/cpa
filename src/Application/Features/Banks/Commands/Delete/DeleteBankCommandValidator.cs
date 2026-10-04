namespace BigLion.CPA.Application.Features.Banks.Commands.Delete;

public class DeleteBankCommandValidator : AbstractValidator<DeleteBankCommand>
{
    public DeleteBankCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
