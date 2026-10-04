namespace BigLion.CPA.Application.Features.UserLogFiles.Queries.Get;

public class GetUserLogFileQueryValidator : AbstractValidator<GetUserLogFileQuery>
{
    public GetUserLogFileQueryValidator()
    {
        RuleFor(x => x.LogID).GreaterThan(0);
    }
}
