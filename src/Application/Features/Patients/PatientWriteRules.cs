using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Patients;

public interface IPatientWrite
{
    string? Gender { get; }
    string? CardId { get; }
    string? Telephone { get; }
    string? Mobile { get; }
    string? TimeContact { get; }
    string? AddressType { get; }
    string? AddressNo { get; }
    string? Road { get; }
    string? DistrictId { get; }
    string? City { get; }
    string? ProvinceId { get; }
    string? ProvinceName { get; }
    string? ZipCode { get; }
    string? MainClaim { get; }
    string? Education { get; }
    string? Occupation { get; }
    string? DrugAllergy { get; }
    string? SmokingRemark { get; }
}

internal sealed class PatientWriteRules : AbstractValidator<IPatientWrite>
{
    public PatientWriteRules(ICpaDatabaseContext context)
    {
        RuleFor(p => p.Gender)
            .MaximumLength(20).WithMessage("เพศต้องไม่เกิน 20 ตัวอักษร");

        RuleFor(p => p.CardId)
            .MaximumLength(20).WithMessage("เลขบัตรต้องไม่เกิน 20 ตัวอักษร");

        RuleFor(p => p.Telephone)
            .MaximumLength(50).WithMessage("เบอร์โทรศัพท์ต้องไม่เกิน 50 ตัวอักษร");

        RuleFor(p => p.Mobile)
            .MaximumLength(50).WithMessage("เบอร์มือถือต้องไม่เกิน 50 ตัวอักษร");

        RuleFor(p => p.TimeContact)
            .MaximumLength(100).WithMessage("เวลาที่ติดต่อได้ต้องไม่เกิน 100 ตัวอักษร");

        RuleFor(p => p.AddressType)
            .MaximumLength(50).WithMessage("ประเภทที่อยู่ต้องไม่เกิน 50 ตัวอักษร");

        RuleFor(p => p.AddressNo)
            .MaximumLength(500).WithMessage("ที่อยู่ต้องไม่เกิน 500 ตัวอักษร");

        RuleFor(p => p.Road)
            .MaximumLength(200).WithMessage("ถนนต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.City)
            .MaximumLength(200).WithMessage("เมืองต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.ProvinceName)
            .MaximumLength(200).WithMessage("ชื่อจังหวัดต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.ZipCode)
            .MaximumLength(10).WithMessage("รหัสไปรษณีย์ต้องไม่เกิน 10 ตัวอักษร");

        RuleFor(p => p.MainClaim)
            .MaximumLength(200).WithMessage("สิทธิหลักต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.Education)
            .MaximumLength(200).WithMessage("การศึกษาต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.Occupation)
            .MaximumLength(200).WithMessage("อาชีพต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.DrugAllergy)
            .MaximumLength(2000).WithMessage("ประวัติแพ้ยาต้องไม่เกิน 2000 ตัวอักษร");

        RuleFor(p => p.SmokingRemark)
            .MaximumLength(2000).WithMessage("หมายเหตุการสูบบุหรี่ต้องไม่เกิน 2000 ตัวอักษร");

        RuleFor(p => p.ProvinceId)
            .MaximumLength(10).WithMessage("รหัสจังหวัดต้องไม่เกิน 10 ตัวอักษร")
            .MustAsync((id, cancellationToken) => context.Provinces.AsNoTracking().AnyAsync(x => x.Id == id, cancellationToken))
            .When(p => !string.IsNullOrWhiteSpace(p.ProvinceId))
            .WithMessage("ไม่พบจังหวัดที่เลือก");

        RuleFor(p => p.DistrictId)
            .MaximumLength(10).WithMessage("รหัสอำเภอต้องไม่เกิน 10 ตัวอักษร")
            .MustAsync((id, cancellationToken) => context.Districts.AsNoTracking().AnyAsync(x => x.Id == id, cancellationToken))
            .When(p => !string.IsNullOrWhiteSpace(p.DistrictId))
            .WithMessage("ไม่พบอำเภอที่เลือก");

        RuleFor(p => p)
            .MustAsync(async (command, cancellationToken) =>
            {
                var district = await context.Districts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == command.DistrictId, cancellationToken);
                return district == null || district.ProvinceId == command.ProvinceId;
            })
            .When(p => !string.IsNullOrWhiteSpace(p.DistrictId) && !string.IsNullOrWhiteSpace(p.ProvinceId))
            .WithMessage("อำเภอไม่อยู่ภายใต้จังหวัดที่เลือก");
    }
}
