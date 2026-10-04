namespace BigLion.CPA.Application.Features.PaymentMethods.Queries.Get;

public class GetPaymentMethodQueryValidator : AbstractValidator<GetPaymentMethodQuery>
{
    public GetPaymentMethodQueryValidator()
    {
        RuleFor(x => x.PaymentID).GreaterThan(0);
    }
}
