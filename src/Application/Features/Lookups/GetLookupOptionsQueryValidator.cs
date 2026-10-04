namespace BigLion.CPA.Application.Features.Lookups;

public class GetLookupOptionsQueryValidator : AbstractValidator<GetLookupOptionsQuery>
{
    public GetLookupOptionsQueryValidator()
    {
        RuleFor(x => x.Category).NotEmpty();
    }
}
