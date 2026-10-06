using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Patients;

namespace BigLion.CPA.Application.Features.Patients.Commands.Update;

public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientCommandValidator(ICpaDatabaseContext context)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("โปรดระบุ Id");

        RuleFor(p => p.ForeName)
            .NotEmpty().WithMessage("ชื่อต้องไม่ว่าง")
            .MaximumLength(200).WithMessage("ชื่อต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p.Surname)
            .NotEmpty().WithMessage("นามสกุลต้องไม่ว่าง")
            .MaximumLength(200).WithMessage("นามสกุลต้องไม่เกิน 200 ตัวอักษร");

        RuleFor(p => p).SetValidator(new PatientWriteRules(context));
    }
}
