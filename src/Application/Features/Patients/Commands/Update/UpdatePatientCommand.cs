using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;

namespace BigLion.CPA.Application.Features.Patients.Commands.Update;

public record UpdatePatientCommand : IRequest<Unit>
{
    public required int Id { get; set; }
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
    //public int? OrganizationId { get; set; }
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
    public string? ChronicDisease { get; set; }
}


public class UpdateCommandHandler : IRequestHandler<UpdatePatientCommand, Unit>
{
    private readonly ICpaDatabaseContext _context;

    public UpdateCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
        var patient = await _context.Patients.FirstOrDefaultAsync(b => b.Id.Equals(request.Id), cancellationToken);
        if (patient == null)
        {
            throw new NotFoundException(nameof(Patients), request.Id);
        }
     
        patient.HospitalNumber = request.HospitalNumber;
        patient.Prefix = request.Prefix;
        patient.FirstName = request.FirstName;
        patient.MiddleName = request.MiddleName;
        patient.LastName = request.LastName;
        patient.Gender = request.Gender;
        patient.BirthDate = request.BirthDate;
        patient.BloodGroup = request.BloodGroup;
        patient.NationId = request.NationId;
        patient.AddressNo = request.Address;
        patient.DistrictId = request.DistrictId;
        patient.SubDistrictId = request.SubDistrictId;
        patient.ProvinceId = request.ProvinceId;
        patient.DrugAllergy = request.DrugAllergy;
       
        patient.TelephoneNumber = request.TelephoneNumber;     
        patient.ZipCode = request.ZipCode;
        patient.ChronicDisease = request.ChronicDisease;

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
