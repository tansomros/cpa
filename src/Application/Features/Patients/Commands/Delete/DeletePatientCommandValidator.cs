using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Patients.Commands.Delete
{
    public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
    {
        private readonly ICpaDatabaseContext _context;
        public DeletePatientCommandValidator(ICpaDatabaseContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id �������ö�繤����ҧ��").NotNull().WithMessage("�ô�к� Id");
        }
    }
}
