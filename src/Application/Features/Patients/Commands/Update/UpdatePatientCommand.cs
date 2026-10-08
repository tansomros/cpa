using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Application.Exceptions;
using BigLion.CPA.Application.Features.Patients;

namespace BigLion.CPA.Application.Features.Patients.Commands.Update;

public record UpdatePatientCommand : IRequest<Unit>, IPatientWrite
{
    public required int Id { get; set; }
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

    public bool? IsSmoke { get; set; }
    public string? Smoke { get; set; }
    public int? SmokeYear { get; set; }
    public int? SmokeCigarette { get; set; }
    public string? CigaretteType { get; set; }
    public bool? SmokingQuit { get; set; }
    public string? SmokingRemark { get; set; }

    public string? Alcohol { get; set; }
    public int? AlcoholFQ { get; set; }
    public bool IsActive { get; set; }

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
        var patient = await _context.Patients.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Patients), request.Id);

        patient.UpdatePersonalInformation(
            request.ForeName,
            request.Surname,
            request.Gender,
            request.BirthDate,
            request.CardId);
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
            request.IsSmoke,
            request.Smoke,
            request.SmokeYear,
            request.SmokeCigarette,
            request.CigaretteType,
            request.SmokingQuit,
            request.SmokingRemark);
        patient.UpdateAlcoholHistory(request.Alcohol, request.AlcoholFQ);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
