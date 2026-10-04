using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Hospital;

namespace BigLion.CPA.Application.Features.Hospitals.ViewModels;

public class HospitalViewModel : IMapFrom<Entity>
{
    public int HospitalUID { get; set; }
    public string? HospitalName { get; set; }
    public int? HospitalGroupID { get; set; }
    public int? HospitalTypeID { get; set; }
    public int? DepartmentID { get; set; }
    public string? DepartmentName { get; set; }
    public int? LevelID { get; set; }
    public int? Bed { get; set; }
    public int? Branch { get; set; }
    public string? Office_hours { get; set; }
    public int? Officer_Count { get; set; }
    public string? Address { get; set; }
    public string? ProvinceID { get; set; }
    public string? ProvinceName { get; set; }
    public string? ZipCode { get; set; }
    public string? Office_Tel { get; set; }
    public string? Office_Fax { get; set; }
    public string? Co_Name { get; set; }
    public string? Co_Position { get; set; }
    public string? Co_Mail { get; set; }
    public string? Co_Tel { get; set; }
    public string? StatusFlag { get; set; }
    public string? Bill_Name { get; set; }
    public int? WorkDayID { get; set; }
    public string? WorkDayDesc { get; set; }
    public int? WorkTimeID { get; set; }
    public string? WorkTimeDesc { get; set; }
    public string? ConfirmHold { get; set; }
    public int? isBranch { get; set; }
    public string? BranchRemark { get; set; }
    public string? WorkList { get; set; }
    public string? WorkSpec { get; set; }
    public string? WorkTop { get; set; }
    public string? Remark { get; set; }
    public string? Informant { get; set; }
    public string? InfoPosition { get; set; }
    public string? InfoDate { get; set; }
    public string? LetterTo { get; set; }
    public string? Country { get; set; }
    public string? ZoneID { get; set; }
    public string? OfficeID { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
    public DateTime? MWhen { get; set; }
    public string? MUser { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, HospitalViewModel>();
    }
}
