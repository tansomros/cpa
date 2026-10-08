using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTM;

namespace BigLion.CPA.Application.Features.MTMs.ViewModels;

public class MTMViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
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
    public string? Smoke { get; set; }
    public int? SmokeYear { get; set; }
    public int? SmokeCigarette { get; set; }
    public string? CigaretteType { get; set; }
    public string? Alcohol { get; set; }
    public string? AlcoholFQ { get; set; }
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

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, MTMViewModel>();
    }
}
