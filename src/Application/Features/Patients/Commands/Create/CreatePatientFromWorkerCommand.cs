using Cpa.Application.Common.Interfaces;
using Cpa.Domain.Entities;

namespace Cpa.Application.Features.Patients.Commands.Create;
public class CreatePatientFromWorkerCommand : IRequest<int>
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

public class CreatePatientFromWorkerCommandHandler(ICpaDatabaseContext context) : IRequestHandler<CreatePatientFromWorkerCommand, int>
{
    private readonly ICpaDatabaseContext _context = context;

    public async Task<int> Handle(CreatePatientFromWorkerCommand request, CancellationToken cancellationToken)
    {
        var patientExist = await _context.Patients.FirstOrDefaultAsync(
            s => s.HospitalNumber == request.HospitalNumber, cancellationToken
            );

        if ( patientExist != null )
        {
            return patientExist.Id;
        }

        //HosXP ไม่มี MiddleName
        string middleName = string.Empty;


        var subDistrict = await _context.SubDistricts.FirstOrDefaultAsync(x => x.Name == request.SubDistrict, cancellationToken);
        var district = await _context.Districts.FirstOrDefaultAsync(x => x.Name == request.District, cancellationToken);
        var province = await _context.SubDistricts.FirstOrDefaultAsync(x => x.Name == request.Province, cancellationToken);


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
            CreatedOn = DateTime.Now,
            DistrictId = district?.DistrictId,
            SubDistrictId = subDistrict?.SubDistrictId,
            ProvinceId = province?.ProvinceId,
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
}
