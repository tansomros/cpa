namespace BigLion.CPA.Application.Features.Banks.Queries.Get;

public class GetBankQueryValidator : AbstractValidator<GetBankQuery>
{
    public GetBankQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
