namespace BigLion.CPA.Application.Features.PaymentConfigs.Queries.Get;

public class GetPaymentConfigQueryValidator : AbstractValidator<GetPaymentConfigQuery>
{
    public GetPaymentConfigQueryValidator()
    {
        RuleFor(x => x.itemID).GreaterThan(0);
    }
}
