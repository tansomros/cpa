namespace BigLion.CPA.Application.Features.DrugProblemItems.Queries.Get;

public class GetDrugProblemItemQueryValidator : AbstractValidator<GetDrugProblemItemQuery>
{
    public GetDrugProblemItemQueryValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
    }
}
