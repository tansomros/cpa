using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Cpa.Application.Features.Patients.Commands.Create;
public class CreatePatientFromWorkerCommandValidator : AbstractValidator<CreatePatientFromWorkerCommand>
{
    public CreatePatientFromWorkerCommandValidator()
    {

        RuleFor(p => p.HospitalNumber)
            .NotNull().WithMessage("HN ต้องไม่ว่าง")
            .MaximumLength(8).WithMessage("HN ต้องยาว 8 ตัวเท่านั้น");

        RuleFor(p => p.Prefix)
            .NotEmpty().WithMessage("Prefix ไม่สามารถเป็นค่าว่างได้")
            .NotNull().WithMessage("โปรดระบุ คำนำหน้าชื่อ");

        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("FirstName ไม่สามารถเป็นค่าว่างได้")
            .NotNull().WithMessage("โปรดระบุ ชื่อ");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("LastName ไม่สามารถเป็นค่าว่างได้")
            .NotNull().WithMessage("โปรดระบุ นามสกุล");

        RuleFor(p => p.Gender)
            .NotEmpty().WithMessage("Gender ไม่สามารถเป็นค่าว่างได้")
            .NotNull().WithMessage("โปรดระบุ เพศ");
    }
}
