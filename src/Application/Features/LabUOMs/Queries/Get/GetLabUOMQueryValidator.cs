namespace BigLion.CPA.Application.Features.LabUOMs.Queries.Get;

public class GetLabUOMQueryValidator : AbstractValidator<GetLabUOMQuery>
{
    public GetLabUOMQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
