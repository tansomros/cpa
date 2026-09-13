using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cpa.Application.Common.Interfaces;

namespace Cpa.Application.Features.Patients.Commands.Delete
{
    public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
    {
        private readonly ICpaDatabaseContext _context;
        public DeletePatientCommandValidator(ICpaDatabaseContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id ไม่สามารถเป็นค่าว่างได้").NotNull().WithMessage("โปรดระบุ Id");
        }
    }
}
