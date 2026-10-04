namespace BigLion.CPA.Application.Features.DrugProblemGroups.Queries.Get;

public class GetDrugProblemGroupQueryValidator : AbstractValidator<GetDrugProblemGroupQuery>
{
    public GetDrugProblemGroupQueryValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
