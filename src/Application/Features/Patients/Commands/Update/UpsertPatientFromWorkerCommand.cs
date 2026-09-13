using Cpa.Application.Common.Interfaces;
using Cpa.Application.Exceptions;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Patients.Commands.Update;
public class UpsertPatientFromWorkerCommand : IRequest<int>
{
    public required string HospitalNumber { get; set; }
    public required string Prefix { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? FullName { get; set; }
    public required string Gender { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? NationId { get; set; }
    public string? Nationality { get; set; }
    public string? Religion { get; set; }
    public string? BloodGroup { get; set; }
    public string? EmployeeId { get; set; }
    public string? Address { get; set; }
    public string? SubDistrict { get; set; }
    public string? District { get; set; }
    public string? Province { get; set; }
    public string? ZipCode { get; set; }
    public string? TelephoneNumber { get; set; }
    public string? DrugAllergy { get; set; }
    public string? ChronicDisease { get; set; }
}



public class UpsertPatientFromWorkerCommandHandler(ICpaDatabaseContext context) : IRequestHandler<UpsertPatientFromWorkerCommand, int>
{
    private readonly ICpaDatabaseContext _context = context;

    public async Task<int> Handle(UpsertPatientFromWorkerCommand request, CancellationToken cancellationToken)
    {
        var patientExist = await _context.Patients.FirstOrDefaultAsync(
            s => s.HospitalNumber == request.HospitalNumber, cancellationToken
            );

        //HosXP ไม่มี MiddleName
        string middleName = string.Empty;
        //var subDistrict = await _context.SubDistricts.FirstOrDefaultAsync(x => x.Name == request.SubDistrict && x., cancellationToken);
        //var district = await _context.Districts.FirstOrDefaultAsync(x => x.Name == request.District, cancellationToken);
        var province = await _context.Provinces.FirstOrDefaultAsync(x => x.Name == request.Province, cancellationToken);

        var district = await _context.Districts.FirstOrDefaultAsync(
            x => 
            x.Name == request.District && province != null && x.ProvinceId == province.Id
            , cancellationToken);

        var subDistrict = await _context.SubDistricts.FirstOrDefaultAsync(
            x => x.Name == request.SubDistrict && district != null && x.DistrictId==district.DistrictId && province != null && x.ProvinceId == province.Id, cancellationToken);

        if (patientExist == null)
        {

            var patient = new Patient(
                    request.HospitalNumber,
                    request.Prefix,
                    request.FirstName,
                    middleName,
                    request.LastName,
                    request.Gender,
                    request.BirthDate)
            {
                BloodGroup = request.BloodGroup,
                NationId = request.NationId,
                Nationality = request.Nationality,
                Address = request.Address,
                EmployeeId = request.EmployeeId,
                //CreatedOn = DateTime.Now,
                DistrictId = district?.DistrictId,
                SubDistrictId = subDistrict?.SubDistrictId,
                ProvinceId = province?.Id,
                DrugAllergy = request.DrugAllergy,
                TelephoneNumber = request.TelephoneNumber,
                Religious = request.Religion,
                ZipCode = request.ZipCode,
                ChronicDisease = request.ChronicDisease,
                IsActive = true
            };
            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken); 
            return patient.Id;
        }
        else
        {
            patientExist.HospitalNumber = request.HospitalNumber;
            patientExist.Prefix = request.Prefix;
            patientExist.FirstName = request.FirstName;
            patientExist.MiddleName = middleName;
            patientExist.LastName = request.LastName;
            patientExist.Gender = request.Gender;
            patientExist.BirthDate = request.BirthDate;
            patientExist.BloodGroup = request.BloodGroup;
            patientExist.NationId = request.NationId;
            patientExist.Nationality = request.Nationality;
            patientExist.Address = request.Address;
            patientExist.EmployeeId = request.EmployeeId;
            //patientExist.CreatedOn = DateTime.Now;
            patientExist.DistrictId = district?.DistrictId;
            patientExist.SubDistrictId = subDistrict?.SubDistrictId;
            patientExist.ProvinceId = province?.Id;
            patientExist.DrugAllergy = request.DrugAllergy;
            patientExist.TelephoneNumber = request.TelephoneNumber;
            patientExist.Religious = request.Religion;
            patientExist.ZipCode = request.ZipCode;
            patientExist.ChronicDisease = request.ChronicDisease;

            _context.Patients.Update(patientExist);
            await _context.SaveChangesAsync(cancellationToken);

            return patientExist.Id;
        }       
    }
}
