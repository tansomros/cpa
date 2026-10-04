namespace BigLion.CPA.Application.Features.PaymentConfigs.Commands.Delete;

public class DeletePaymentConfigCommandValidator : AbstractValidator<DeletePaymentConfigCommand>
{
    public DeletePaymentConfigCommandValidator()
    {
        RuleFor(x => x.itemID).GreaterThan(0);
    }
}
