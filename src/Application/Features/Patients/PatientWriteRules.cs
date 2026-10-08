using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Domain.Enums;

namespace BigLion.CPA.Application.Features.Patients;

public interface IPatientWrite
{
    string? Gender { get; }
    DateOnly? BirthDate { get; }
    string? CardId { get; }
    string? Telephone { get; }
    string? TimeContact { get; }
    string? AddressType { get; }
    string? AddressNo { get; }
    string? Road { get; }
    string? DistrictId { get; }
    string? SubDistrictId { get; }
    string? ProvinceId { get; }
    string? ZipCode { get; }
    string? MainClaim { get; }
    string? Education { get; }
    string? Occupation { get; }
    string? DrugAllergy { get; }
    string? Smoke { get; }
    string? CigaretteType { get; }
    string? SmokingRemark { get; }
    string? Alcohol { get; }
    int? AlcoholFQ { get; }
}

internal sealed class PatientWriteRules : AbstractValidator<IPatientWrite>
{
    /// <summary>Oldest allowed birth date is today minus this many years.</summary>
    public const int MaxAgeYears = 120;

    /// <summary>Drink frequency is the number of drinking days per week (0-7).</summary>
    public const int MaxDrinkDaysPerWeek = 7;

    public PatientWriteRules(ICpaDatabaseContext context, TimeProvider timeProvider)
    {
        // BirthDate is optional; when given it must be between (today - 120 years) and today.
        // "Today" comes from TimeProvider so the rule can be tested with a fixed date.
        RuleFor(p => p.BirthDate)
            .Must(birthDate => birthDate!.Value <= Today(timeProvider))
            .WithMessage("วันเกิดต้องไม่เกินวันที่ปัจจุบัน")
            .Must(birthDate => birthDate!.Value >= Today(timeProvider).AddYears(-MaxAgeYears))
            .WithMessage("วันเกิดต้องไม่เกิน 120 ปีนับจากวันนี้")
            .When(p => p.BirthDate.HasValue);
        RuleFor(p => p.Gender)
            .MaximumLength(20).WithMessage("เพศต้องไม่เกิน 20 ตัวอักษร");

        RuleFor(p => p.CardId)
            .MaximumLength(20).WithMessage("เลขบัตรต้องไม่เกิน 20 ตัวอักษร");

        RuleFor(p => p.Telephone)
            .MaximumLength(50).WithMessage("เบอร์โทรศัพท์ต้องไม่เกิน 50 ตัวอักษร");

        RuleFor(p => p.TimeContact)
            .MaximumLength(100).WithMessage("เวลาที่ติดต่อได้ต้องไม่เกิน 100 ตัวอักษร");

        RuleFor(p => p.AddressType)
            .MaximumLength(50).WithMessage("ประเภทที่อยู่ต้องไม่เกิน 50 ตัวอักษร");

        RuleFor(p => p.AddressNo)
            .MaximumLength(500).WithMessage("ที่อยู่ต้องไม่เกิน 500 ตัวอักษร");

        RuleFor(p => p.Road)
            .MaximumLength(200).WithMessage("ถนนต้องไม่เกิน 200 ตัวอักษร");

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

        // Lookup codes must match a SmartEnum value exactly (case-sensitive), so the
        // database only ever holds the canonical codes served by /Options.
        RuleFor(p => p.Smoke)
            .Must(code => SmokingValue.All.Any(x => string.Equals(x.Value, code, StringComparison.Ordinal)))
            .WithMessage("รหัสการสูบบุหรี่ไม่ถูกต้อง")
            .When(p => p.Smoke is not null);

        RuleFor(p => p.CigaretteType)
            .Must(code => CigaretteTypeValue.All.Any(x => string.Equals(x.Value, code, StringComparison.Ordinal)))
            .WithMessage("รหัสชนิดบุหรี่ไม่ถูกต้อง")
            .When(p => p.CigaretteType is not null);

        RuleFor(p => p.Alcohol)
            .Must(code => DrinkingValue.All.Any(x => string.Equals(x.Value, code, StringComparison.Ordinal)))
            .WithMessage("รหัสการดื่มไม่ถูกต้อง")
            .When(p => p.Alcohol is not null);

        // Drink frequency (days per week) is only checked when it is kept, i.e. for
        // Occasional or Regular drinkers. For other statuses the Domain clears it,
        // so a leftover value is not an error (see Patient.UpdateAlcoholHistory).
        RuleFor(p => p.AlcoholFQ)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("ดื่มประจำต้องระบุจำนวนวันที่ดื่มต่อสัปดาห์ตั้งแต่ 1 วันขึ้นไป")
            .GreaterThanOrEqualTo(1).WithMessage("ดื่มประจำต้องระบุจำนวนวันที่ดื่มต่อสัปดาห์ตั้งแต่ 1 วันขึ้นไป")
            .When(p => IsDrinking(p.Alcohol, DrinkingValue.Regular));

        RuleFor(p => p.AlcoholFQ)
            .InclusiveBetween(0, MaxDrinkDaysPerWeek).WithMessage("ความถี่การดื่มต้องอยู่ระหว่าง 0 ถึง 7 วันต่อสัปดาห์")
            .When(p => p.AlcoholFQ.HasValue
                && (IsDrinking(p.Alcohol, DrinkingValue.Regular) || IsDrinking(p.Alcohol, DrinkingValue.Occasional)));

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

        RuleFor(p => p.SubDistrictId)
            .MaximumLength(10).WithMessage("รหัสตำบลต้องไม่เกิน 10 ตัวอักษร")
            .MustAsync((id, cancellationToken) => context.SubDistricts.AsNoTracking().AnyAsync(x => x.SubDistrictId == id, cancellationToken))
            .When(p => !string.IsNullOrWhiteSpace(p.SubDistrictId))
            .WithMessage("ไม่พบตำบลที่เลือก");

        RuleFor(p => p.SubDistrictId)
            .MustAsync(async (command, subDistrictId, cancellationToken) =>
            {
                var subDistrict = await context.SubDistricts.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SubDistrictId == subDistrictId, cancellationToken);
                return subDistrict == null || subDistrict.DistrictId == command.DistrictId;
            })
            .When(p => !string.IsNullOrWhiteSpace(p.SubDistrictId) && !string.IsNullOrWhiteSpace(p.DistrictId))
            .WithMessage("ตำบลไม่อยู่ภายใต้อำเภอที่เลือก");
    }

    private static bool IsDrinking(string? code, DrinkingValue status)
        => code is not null && DrinkingValue.TryFromValue(code, out var value) && value == status;

    private static DateOnly Today(TimeProvider timeProvider) =>
        DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);
}
