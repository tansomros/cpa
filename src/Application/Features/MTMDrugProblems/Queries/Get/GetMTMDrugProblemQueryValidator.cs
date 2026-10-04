namespace BigLion.CPA.Application.Features.MTMDrugProblems.Queries.Get;

public class GetMTMDrugProblemQueryValidator : AbstractValidator<GetMTMDrugProblemQuery>
{
    public GetMTMDrugProblemQueryValidator()
    {
        RuleFor(x => x.UID).GreaterThan(0);
    }
}
