using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Features.Patients;
using BigLion.CPA.Domain.Entities;

namespace BigLion.CPA.Application.Features.Patients.Commands.Create;

public class CreatePatientCommand : IRequest<int>, IPatientWrite
{
    public string? ForeName { get; set; }
    public string? Surname { get; set; }
    public string? Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? CardId { get; set; }

    public string? Telephone { get; set; }
    public string? TimeContact { get; set; }

    public string? AddressType { get; set; }
    public string? AddressNo { get; set; }
    public string? Road { get; set; }
    public string? DistrictId { get; set; }
    public string? City { get; set; }
    public string? ProvinceId { get; set; }
    public string? ZipCode { get; set; }

    public string? MainClaim { get; set; }
 
    public string? Education { get; set; }
    public string? Occupation { get; set; }

    public bool? IsAllergy { get; set; }
    public string? DrugAllergy { get; set; }

    public string? Smoke { get; set; }
    public int? SmokeYear { get; set; }
    public int? SmokeCigarette { get; set; }
    public string? CigaretteType { get; set; }
    public bool? SmokingQuit { get; set; }
    public string? SmokingRemark { get; set; }

    public string? Alcohol { get; set; }
    public int? AlcoholFQ { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreatePatientCommmandHandler : IRequestHandler<CreatePatientCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreatePatientCommmandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = new Patient(           
            request.ForeName,
            request.Surname,
            request.CardId,
            request.Gender,
            request.BirthDate);

        patient.UpdateContact(request.Telephone, request.TimeContact);
        patient.UpdateAddress(
            request.AddressType,
            request.AddressNo,
            request.Road,
            request.DistrictId,
            request.City,
            request.ProvinceId,
            request.ZipCode);
        patient.UpdateGeneralInformation(request.MainClaim, request.IsActive, request.Education, request.Occupation);
        patient.UpdateAllergy(request.IsAllergy, request.DrugAllergy);
        patient.UpdateSmokingHistory(
            request.Smoke,
            request.SmokeYear,
            request.SmokeCigarette,
            request.CigaretteType,
            request.SmokingQuit,
            request.SmokingRemark);
        patient.UpdateAlcoholHistory(request.Alcohol, request.AlcoholFQ);

        await _context.Patients.AddAsync(patient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return patient.Id;
    }
}
