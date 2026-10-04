using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.SubDistrict;

namespace BigLion.CPA.Application.Features.SubDistricts.ViewModels;

public class SubDistrictViewModel : IMapFrom<Entity>
{
    public string ProvinceId { get; set; } = string.Empty;
    public string DistrictId { get; set; } = string.Empty;
    public string SubDistrictId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, SubDistrictViewModel>();
    }
}
