namespace BigLion.CPA.Application.Features.LabItems.Queries.Get;

public class GetLabItemQueryValidator : AbstractValidator<GetLabItemQuery>
{
    public GetLabItemQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
