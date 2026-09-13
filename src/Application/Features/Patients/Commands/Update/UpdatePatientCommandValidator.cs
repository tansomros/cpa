using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Patients.Commands.Update
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        private readonly ICpaDatabaseContext _context;
        public UpdatePatientCommandValidator(ICpaDatabaseContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id ไม่สามารถเป็นค่าว่างได้").NotNull().WithMessage("โปรดระบุ Id");
            RuleFor(x => x.HospitalNumber).NotEmpty().WithMessage("HN ไม่สามารถเป็นค่าว่างได้").NotNull().WithMessage("โปรดระบุ HN");
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("ชื่อ ไม่สามารถเป็นค่าว่างได้").NotNull().WithMessage("กรุณาระบุชื่อ");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("นามสกุล ไม่สามารถเป็นค่าว่างได้").NotNull().WithMessage("กรุณาระบุนามสกุล");
        }
    }
}
