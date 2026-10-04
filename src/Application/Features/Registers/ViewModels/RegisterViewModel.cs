using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Register;

namespace BigLion.CPA.Application.Features.Registers.ViewModels;

public class RegisterViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public string? LocationID { get; set; }
    public string? LicenseNo { get; set; }
    public string? LocationName { get; set; }
    public string? LocationName2 { get; set; }
    public string? NHSOCode { get; set; }
    public string? LocationType { get; set; }
    public string? LocationGroupID { get; set; }
    public string? Address { get; set; }
    public string? ProvinceID { get; set; }
    public string? ProvinceName { get; set; }
    public string? ZipCode { get; set; }
    public string? Office_Tel { get; set; }
    public string? Office_Mail { get; set; }
    public string? Office_Hour { get; set; }
    public string? LineID { get; set; }
    public string? Co_Name { get; set; }
    public string? Co_LicenseNo { get; set; }
    public string? Co_Mail { get; set; }
    public string? Co_Tel { get; set; }
    public string? RegisYear { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
    public int? RegisterStatus { get; set; }
    public DateTime? RegisterDate { get; set; }
    public int? MUser { get; set; }
    public DateTime? MWhen { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, RegisterViewModel>();
    }
}
