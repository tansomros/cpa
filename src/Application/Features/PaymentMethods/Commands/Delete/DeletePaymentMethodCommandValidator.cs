namespace BigLion.CPA.Application.Features.PaymentMethods.Commands.Delete;

public class DeletePaymentMethodCommandValidator : AbstractValidator<DeletePaymentMethodCommand>
{
    public DeletePaymentMethodCommandValidator()
    {
        RuleFor(x => x.PaymentID).GreaterThan(0);
    }
}
