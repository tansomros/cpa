using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Patients.Commands.Create
{
    // input หรือ request
    public class CreatePatientCommand : IRequest<int>
    {
        public required string HospitalNumber { get; set; }
        public required string Prefix { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public required string Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? NationId { get; set; }
        public string? Nationality { get; set; }
        public string? Religious { get; set; }
        public string? BloodGroup { get; set; }
        public string? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public string? Address { get; set; }
        public string? SubDistrictId { get; set; }
        public string? DistrictId { get; set; }
        public string? ProvinceId { get; set; }
        public string? ZipCode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? DrugAllergy { get; set; }
        public string? PrefixEnglish { get; set; }
        public string? FirstNameEnglish { get; set; }
        public string? LastNameEnglish { get; set; }
        public string? MiddleNameEnglish { get; set; }
        public string? NationalityEnglish { get; set; }
        public string? ReligiousEnglish { get; set; }
        public string? AddressEnglish { get; set; }
        public string? ChronicDisease {  get; set; }
    }

    // เอา INPUT มา Process
    public class CreatePatientCommmandHandler : IRequestHandler<CreatePatientCommand, int>
    {
        private readonly ICpaDatabaseContext _checkupDatabaseContext;

        public CreatePatientCommmandHandler(ICpaDatabaseContext checkupDatabaseContext)
        {
            _checkupDatabaseContext = checkupDatabaseContext;
        }

        public async Task<int> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient(
                request.HospitalNumber,
                request.Prefix,
                request.FirstName,
                request.MiddleName,
                request.LastName,
                request.Gender,
                request.BirthDate)
            {
                BloodGroup = request.BloodGroup,
                NationId = request.NationId,
                Nationality = request.Nationality,
                NationalityEnglish = request.NationalityEnglish,
                Address = request.Address,
                AddressEnglish = request.AddressEnglish,
                //CompanyId = request.CompanyId,
                EmployeeId = request.EmployeeId,
                DistrictId = request.DistrictId,
                SubDistrictId = request.SubDistrictId,
                ProvinceId = request.ProvinceId,
                DrugAllergy = request.DrugAllergy,
                FirstNameEnglish = request.FirstNameEnglish,
                LastNameEnglish = request.LastNameEnglish,
                TelephoneNumber = request.TelephoneNumber,
                Religious = request.Religious,
                ReligiousEnglish = request.ReligiousEnglish,
                PrefixEnglish = request.PrefixEnglish,
                ZipCode = request.ZipCode,
                MiddleNameEnglish = request.MiddleNameEnglish,
                ChronicDisease = request.ChronicDisease
            };

           
            await _checkupDatabaseContext.Patients.AddAsync(patient, cancellationToken);
            await _checkupDatabaseContext.SaveChangesAsync(cancellationToken);
            return patient.Id;
        }
    }
}
