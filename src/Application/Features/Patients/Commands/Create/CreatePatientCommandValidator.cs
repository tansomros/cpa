using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Patients;

namespace BigLion.CPA.Application.Features.Patients.Commands.Create;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator(ICpaDatabaseContext context)
    {
        RuleFor(p => p.ForeName)
            .NotEmpty().WithMessage("ชื่อต้องไม่ว่าง")
            .MaximumLength(200).WithMessage("ชื่อต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.Surname)
            .NotEmpty().WithMessage("นามสกุลต้องไม่ว่าง")
            .MaximumLength(200).WithMessage("นามสกุลต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p).SetValidator(new PatientWriteRules(context));
    }
}
