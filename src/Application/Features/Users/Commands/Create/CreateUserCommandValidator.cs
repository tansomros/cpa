using System;
using System.Collections.Generic;
using System.Text;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Users.Commands.Create;


public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly ICpaDatabaseContext _context;

    public CreateUserCommandValidator(ICpaDatabaseContext context)
    {
        _context = context;

        RuleFor(p => p.Username)
            .NotEmpty().WithMessage("ชื่อผู้ใช้เป็นค่าว่างไม่ได้")
            .MaximumLength(50).WithMessage("ชื่อผู้ใช้ต้องไม่เกิน 50 ตัวอักษร")
            .MustAsync(BeUniqueUsername).WithMessage("ชื่อผู้ใช้นี้มีอยู่แล้ว");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("รหัสผ่านเป็นค่าว่างไม่ได้")
            .MinimumLength(4).WithMessage("รหัสผ่านต้องมีอย่างน้อย 4 ตัวอักษร");

        RuleFor(p => p.DisplayName).NotEmpty().WithMessage("ชื่อที่แสดงเป็นค่าว่างไม่ได้");
        RuleFor(p => p.PositionName).NotEmpty().WithMessage("ตำแหน่งเป็นค่าว่างไม่ได้");

        RuleFor(p => p.RoleId)
            .GreaterThan(0).WithMessage("โปรดระบุบทบาท");

        RuleFor(p => p.PharmacyId)
            .GreaterThan(0).WithMessage("โปรดระบุร้านยา")
            .When(p => p.PharmacyId.HasValue);

        //RuleFor(p => p.ReportText).NotEmpty().WithMessage("ReportText เป็นค่าว่างไม่ได้");
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(x => x.Username == username, cancellationToken);
    }
}
