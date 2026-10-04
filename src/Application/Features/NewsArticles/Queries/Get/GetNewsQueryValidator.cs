namespace BigLion.CPA.Application.Features.NewsArticles.Queries.Get;

public class GetNewsQueryValidator : AbstractValidator<GetNewsQuery>
{
    public GetNewsQueryValidator()
    {
        RuleFor(x => x.NewsID).GreaterThan(0);
    }
}
