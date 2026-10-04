namespace BigLion.CPA.Application.Features.MTMDeseases.Queries.Get;

public class GetMTMDeseaseQueryValidator : AbstractValidator<GetMTMDeseaseQuery>
{
    public GetMTMDeseaseQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
