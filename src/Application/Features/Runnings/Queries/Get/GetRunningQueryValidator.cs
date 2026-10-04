namespace BigLion.CPA.Application.Features.Runnings.Queries.Get;

public class GetRunningQueryValidator : AbstractValidator<GetRunningQuery>
{
    public GetRunningQueryValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
