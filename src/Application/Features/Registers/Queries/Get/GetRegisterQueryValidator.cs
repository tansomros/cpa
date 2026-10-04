namespace BigLion.CPA.Application.Features.Registers.Queries.Get;

public class GetRegisterQueryValidator : AbstractValidator<GetRegisterQuery>
{
    public GetRegisterQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
