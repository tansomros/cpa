namespace BigLion.CPA.Application.Features.RunningConfigs.Queries.Get;

public class GetRunningConfigQueryValidator : AbstractValidator<GetRunningConfigQuery>
{
    public GetRunningConfigQueryValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
