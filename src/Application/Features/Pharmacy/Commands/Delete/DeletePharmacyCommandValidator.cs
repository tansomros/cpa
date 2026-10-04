namespace BigLion.CPA.Application.Features.Pharmacy.Commands.Delete;

public class DeletePharmacyCommandValidator : AbstractValidator<DeletePharmacyCommand>
{
    public DeletePharmacyCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("รหัสร้านขายยาต้องไม่ว่าง");

        RuleFor(p => p.RowVersion)
            .NotEmpty().WithMessage("กรุณาระบุ RowVersion ของข้อมูลที่กำลังแก้ไข");
    }
}
