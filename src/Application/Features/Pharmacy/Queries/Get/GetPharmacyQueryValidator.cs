namespace BigLion.CPA.Application.Features.Pharmacy.Queries.Get;

public class GetPharmacyQueryValidator : AbstractValidator<GetPharmacyQuery>
{
    public GetPharmacyQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("รหัสร้านขายยาต้องไม่ว่าง");
    }
}
