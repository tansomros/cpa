using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Patients.Commands.Update
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        private readonly ICpaDatabaseContext _context;
        public UpdatePatientCommandValidator(ICpaDatabaseContext context)
        {
            _context = context;

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id �������ö�繤����ҧ��").NotNull().WithMessage("�ô�к� Id");
            RuleFor(x => x.HospitalNumber).NotEmpty().WithMessage("HN �������ö�繤����ҧ��").NotNull().WithMessage("�ô�к� HN");
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("���� �������ö�繤����ҧ��").NotNull().WithMessage("��س��кت���");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("���ʡ�� �������ö�繤����ҧ��").NotNull().WithMessage("��س��кع��ʡ��");
        }
    }
}
