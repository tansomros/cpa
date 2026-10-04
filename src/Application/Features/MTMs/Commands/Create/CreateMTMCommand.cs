using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTM;

namespace BigLion.CPA.Application.Features.MTMs.Commands.Create;

public class CreateMTMCommand : IRequest<int>
{
    public string? LocationID { get; set; }
    public int? xBYear { get; set; }
    public int PatientID { get; set; }
    public int? SEQ { get; set; }
    public string? MTMTYPE { get; set; }
    public string? PFROM { get; set; }
    public string? FROMTXT { get; set; }
    public int? ServiceDate { get; set; }
    public int? ServiceTime { get; set; }
    public int? PersonID { get; set; }
    public int? Smoke { get; set; }
    public int? SmokeYear { get; set; }
    public int? SmokeCigarette { get; set; }
    public int? CigaretteType { get; set; }
    public int? Alcohol { get; set; }
    public int? AlcoholFQ { get; set; }
    public int? HospitalType { get; set; }
    public string? HospitalName { get; set; }
    public int? Status { get; set; }
    public int? PayDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string? UpdBy { get; set; }
    public string? MedicationUsed1 { get; set; }
    public string? MedicationUsed2 { get; set; }
    public string? MedicationUsed3 { get; set; }
    public string? MedicationUsed4 { get; set; }
    public string? MedicationUsed5 { get; set; }
    public string? MedicationUsed6 { get; set; }
    public string? MedicationUsed7 { get; set; }
    public string? MedicationUsed8 { get; set; }
    public string? MedicationUsed9 { get; set; }
    public string? MedicationUsed10 { get; set; }
    public string? MedicationUsed11 { get; set; }
    public string? MedicationUsed12 { get; set; }
    public string? MedicationUsed13 { get; set; }
    public string? MedicationUsed14 { get; set; }
    public string? MedicationUsed15 { get; set; }
    public string? Frequency1 { get; set; }
    public string? Frequency2 { get; set; }
    public string? Frequency3 { get; set; }
    public string? Frequency4 { get; set; }
    public string? Frequency5 { get; set; }
    public string? Frequency6 { get; set; }
    public string? Frequency7 { get; set; }
    public string? Frequency8 { get; set; }
    public string? Frequency9 { get; set; }
    public string? Frequency10 { get; set; }
    public string? Frequency11 { get; set; }
    public string? Frequency12 { get; set; }
    public string? Frequency13 { get; set; }
    public string? Frequency14 { get; set; }
    public string? Frequency15 { get; set; }
    public string? isPitting { get; set; }
    public string? isWound { get; set; }
    public string? isPeripheral { get; set; }
    public string? Pitting { get; set; }
    public string? Wound { get; set; }
    public string? Peripheral { get; set; }
    public string? PayRecordBy { get; set; }
    public DateTime? RWhen { get; set; }
    public string? ReferStatus { get; set; }
    public string? MTMService { get; set; }
    public string? ServiceRemark { get; set; }
    public string? ServiceRef { get; set; }
    public string? TelepharmacyMethod { get; set; }
    public string? RecordMethod { get; set; }
    public string? RecordLocation { get; set; }
    public string? TelepharmacyRemark { get; set; }
}

public class CreateMTMCommandValidator : AbstractValidator<CreateMTMCommand>
{
    public CreateMTMCommandValidator()
    {
        // scalar fields are optional or value types
    }
}

public class CreateMTMCommandHandler : IRequestHandler<CreateMTMCommand, int>
{
    private readonly ICpaDatabaseContext _context;

    public CreateMTMCommandHandler(ICpaDatabaseContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMTMCommand request, CancellationToken cancellationToken)
    {
        var entity = new Entity();
        entity.LocationID = request.LocationID;
        entity.xBYear = request.xBYear;
        entity.PatientID = request.PatientID;
        entity.SEQ = request.SEQ;
        entity.MTMTYPE = request.MTMTYPE;
        entity.PFROM = request.PFROM;
        entity.FROMTXT = request.FROMTXT;
        entity.ServiceDate = request.ServiceDate;
        entity.ServiceTime = request.ServiceTime;
        entity.PersonID = request.PersonID;
        entity.Smoke = request.Smoke;
        entity.SmokeYear = request.SmokeYear;
        entity.SmokeCigarette = request.SmokeCigarette;
        entity.CigaretteType = request.CigaretteType;
        entity.Alcohol = request.Alcohol;
        entity.AlcoholFQ = request.AlcoholFQ;
        entity.HospitalType = request.HospitalType;
        entity.HospitalName = request.HospitalName;
        entity.Status = request.Status;
        entity.PayDate = request.PayDate;
        entity.CloseDate = request.CloseDate;
        entity.CreateDate = request.CreateDate;
        entity.CreateBy = request.CreateBy;
        entity.LastUpdate = request.LastUpdate;
        entity.UpdBy = request.UpdBy;
        entity.MedicationUsed1 = request.MedicationUsed1;
        entity.MedicationUsed2 = request.MedicationUsed2;
        entity.MedicationUsed3 = request.MedicationUsed3;
        entity.MedicationUsed4 = request.MedicationUsed4;
        entity.MedicationUsed5 = request.MedicationUsed5;
        entity.MedicationUsed6 = request.MedicationUsed6;
        entity.MedicationUsed7 = request.MedicationUsed7;
        entity.MedicationUsed8 = request.MedicationUsed8;
        entity.MedicationUsed9 = request.MedicationUsed9;
        entity.MedicationUsed10 = request.MedicationUsed10;
        entity.MedicationUsed11 = request.MedicationUsed11;
        entity.MedicationUsed12 = request.MedicationUsed12;
        entity.MedicationUsed13 = request.MedicationUsed13;
        entity.MedicationUsed14 = request.MedicationUsed14;
        entity.MedicationUsed15 = request.MedicationUsed15;
        entity.Frequency1 = request.Frequency1;
        entity.Frequency2 = request.Frequency2;
        entity.Frequency3 = request.Frequency3;
        entity.Frequency4 = request.Frequency4;
        entity.Frequency5 = request.Frequency5;
        entity.Frequency6 = request.Frequency6;
        entity.Frequency7 = request.Frequency7;
        entity.Frequency8 = request.Frequency8;
        entity.Frequency9 = request.Frequency9;
        entity.Frequency10 = request.Frequency10;
        entity.Frequency11 = request.Frequency11;
        entity.Frequency12 = request.Frequency12;
        entity.Frequency13 = request.Frequency13;
        entity.Frequency14 = request.Frequency14;
        entity.Frequency15 = request.Frequency15;
        entity.isPitting = request.isPitting;
        entity.isWound = request.isWound;
        entity.isPeripheral = request.isPeripheral;
        entity.Pitting = request.Pitting;
        entity.Wound = request.Wound;
        entity.Peripheral = request.Peripheral;
        entity.PayRecordBy = request.PayRecordBy;
        entity.RWhen = request.RWhen;
        entity.ReferStatus = request.ReferStatus;
        entity.MTMService = request.MTMService;
        entity.ServiceRemark = request.ServiceRemark;
        entity.ServiceRef = request.ServiceRef;
        entity.TelepharmacyMethod = request.TelepharmacyMethod;
        entity.RecordMethod = request.RecordMethod;
        entity.RecordLocation = request.RecordLocation;
        entity.TelepharmacyRemark = request.TelepharmacyRemark;
        await _context.MTMs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.UID;
    }
}
