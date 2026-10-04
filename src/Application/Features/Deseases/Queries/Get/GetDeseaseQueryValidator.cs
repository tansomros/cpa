namespace BigLion.CPA.Application.Features.Deseases.Queries.Get;

public class GetDeseaseQueryValidator : AbstractValidator<GetDeseaseQuery>
{
    public GetDeseaseQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
