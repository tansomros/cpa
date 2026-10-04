using BigLion.CPA.Application.Common.Interfaces;
using PharmacyEntity = BigLion.CPA.Domain.Entities.Pharmacy;

namespace BigLion.CPA.Application.Features.Pharmacy.ViewModels;

public class PharmacyViewModel : IMapFrom<PharmacyEntity>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? LicenseNo { get; set; }
    public string? NhsoCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Name2 { get; set; }
    public int? PharmacyGroupId { get; set; }
    public string? PharmacyGroupName { get; set; }
    public int? PharmacyTypeId { get; set; }
    public string? PharmacyTypeName { get; set; }
    public string? PharmacyTypeOther { get; set; }
    public string? AddressNo { get; set; }
    public string? ProvinceId { get; set; }
    public string? ProvinceName { get; set; }
    public string? DistrictId { get; set; }
    public string? DistrictName { get; set; }
    public string? SubDistrictId { get; set; }
    public string? SubDistrictName { get; set; }
    public string? ZipCode { get; set; }
    public string? Fda_Province { get; set; }
    public string? Office_Tel { get; set; }
    public string? Office_Fax { get; set; }
    public string? Office_Mail { get; set; }
    public string? LineID { get; set; }
    public string? Co_Name { get; set; }
    public string? Co_Mail { get; set; }
    public string? Co_Tel { get; set; }
    public string? RegisYear { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
    public bool IsActive { get; set; }
    public bool? DeleteFlag { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public uint RowVersion { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<PharmacyEntity, PharmacyViewModel>()
            .ForMember(d => d.PharmacyGroupName, opt => opt.MapFrom(s => s.PharmacyGroup != null ? s.PharmacyGroup.Name : null))
            .ForMember(d => d.PharmacyTypeName, opt => opt.MapFrom(s => s.PharmacyType != null ? s.PharmacyType.Name : null))
            .ForMember(d => d.ProvinceName, opt => opt.MapFrom(s => s.Province != null ? s.Province.Name : null))
            .ForMember(d => d.DistrictName, opt => opt.MapFrom(s => s.District != null ? s.District.Name : null))
            .ForMember(d => d.SubDistrictName, opt => opt.MapFrom(s => s.SubDistrict != null ? s.SubDistrict.Name : null))
            .ForMember(d => d.RowVersion, opt => opt.Ignore());
    }
}
