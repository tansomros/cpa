namespace BigLion.CPA.Application.Features.Pharmacy.Queries.Get;

public class GetPharmaciesQueryValidator : AbstractValidator<GetPharmaciesQuery>
{
    public GetPharmaciesQueryValidator()
    {
        RuleFor(p => p.Page)
            .GreaterThanOrEqualTo(1).WithMessage("หน้าต้องมีค่าอย่างน้อย 1");

        RuleFor(p => p.Limit)
            .InclusiveBetween(1, 100).WithMessage("จำนวนต่อหน้าต้องอยู่ระหว่าง 1 ถึง 100");
    }
}
