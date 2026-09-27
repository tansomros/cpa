using System;
using System.Collections.Generic;
using System.Text;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Users.Commands.Create;


public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly ICpaDatabaseContext _context;

    public CreateUserCommandValidator(ICpaDatabaseContext context)
    {
        _context = context;

        RuleFor(p => p.Username).NotEmpty().WithMessage("Username เป็นค่าว่างไม่ได้");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Password เป็นค่าว่างไม่ได้");

        RuleFor(p => p.RoleId)
            .NotEmpty().WithMessage("RoleId ต้องไม่เป็นค่าว่าง")
            .NotNull().WithMessage("RoleId ต้องไม่เป็นค่า NULL");

        //RuleFor(p => p.ReportText).NotEmpty().WithMessage("ReportText เป็นค่าว่างไม่ได้");
    }
}
