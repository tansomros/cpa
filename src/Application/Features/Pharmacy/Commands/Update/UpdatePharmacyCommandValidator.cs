using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Pharmacy.Commands.Update;

public class UpdatePharmacyCommandValidator : AbstractValidator<UpdatePharmacyCommand>
{
    private const int CodeMaxLength = 50;
    private const int NameMaxLength = 300;

    public UpdatePharmacyCommandValidator(ICpaDatabaseContext context)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("รหัสร้านขายยาต้องไม่ว่าง");

        RuleFor(p => p.RowVersion)
            .NotEmpty().WithMessage("กรุณาระบุ RowVersion ของข้อมูลที่กำลังแก้ไข");

        RuleFor(p => p.Code)
            .NotEmpty().WithMessage("รหัสร้านยาต้องไม่ว่าง")
            .MaximumLength(CodeMaxLength).WithMessage($"รหัสร้านยาต้องไม่เกิน {CodeMaxLength} ตัวอักษร");

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("ชื่อร้านยาต้องไม่ว่าง")
            .MaximumLength(NameMaxLength).WithMessage($"ชื่อร้านยาต้องไม่เกิน {NameMaxLength} ตัวอักษร");

        RuleFor(p => p.PharmacyGroupId)
            .MustAsync((id, cancellationToken) => context.PharmacyGroups.AsNoTracking().AnyAsync(g => g.Id == id, cancellationToken))
            .When(p => p.PharmacyGroupId.HasValue)
            .WithMessage("ไม่พบกลุ่มร้านยาที่เลือก");

        RuleFor(p => p.PharmacyTypeId)
            .MustAsync((id, cancellationToken) => context.PharmacyTypes.AsNoTracking().AnyAsync(t => t.Id == id, cancellationToken))
            .When(p => p.PharmacyTypeId.HasValue)
            .WithMessage("ไม่พบประเภทร้านยาที่เลือก");

        RuleFor(p => p.ProvinceId)
            .MustAsync((id, cancellationToken) => context.Provinces.AsNoTracking().AnyAsync(p => p.Id == id, cancellationToken))
            .When(p => !string.IsNullOrWhiteSpace(p.ProvinceId))
            .WithMessage("ไม่พบจังหวัดที่เลือก");

        RuleFor(p => p.DistrictId)
            .MustAsync((id, cancellationToken) => context.Districts.AsNoTracking().AnyAsync(d => d.Id == id, cancellationToken))
            .When(p => !string.IsNullOrWhiteSpace(p.DistrictId))
            .WithMessage("ไม่พบอำเภอที่เลือก");

        RuleFor(p => p.DistrictId)
            .MustAsync(async (command, districtId, cancellationToken) =>
            {
                var district = await context.Districts.AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == districtId, cancellationToken);
                return district == null || district.ProvinceId == command.ProvinceId;
            })
            .When(p => !string.IsNullOrWhiteSpace(p.DistrictId) && !string.IsNullOrWhiteSpace(p.ProvinceId))
            .WithMessage("อำเภอไม่อยู่ภายใต้จังหวัดที่เลือก");

        RuleFor(p => p.SubDistrictId)
            .MustAsync((id, cancellationToken) => context.SubDistricts.AsNoTracking().AnyAsync(s => s.SubDistrictId == id, cancellationToken))
            .When(p => !string.IsNullOrWhiteSpace(p.SubDistrictId))
            .WithMessage("ไม่พบตำบลที่เลือก");

        RuleFor(p => p.SubDistrictId)
            .MustAsync(async (command, subDistrictId, cancellationToken) =>
            {
                var subDistrict = await context.SubDistricts.AsNoTracking()
                    .FirstOrDefaultAsync(s => s.SubDistrictId == subDistrictId, cancellationToken);
                return subDistrict == null || subDistrict.DistrictId == command.DistrictId;
            })
            .When(p => !string.IsNullOrWhiteSpace(p.SubDistrictId) && !string.IsNullOrWhiteSpace(p.DistrictId))
            .WithMessage("ตำบลไม่อยู่ภายใต้อำเภอที่เลือก");
    }
}
