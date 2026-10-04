namespace BigLion.CPA.Application.Features.LabResults.Queries.Get;

public class GetLabResultQueryValidator : AbstractValidator<GetLabResultQuery>
{
    public GetLabResultQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
