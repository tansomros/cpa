using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.District;

namespace BigLion.CPA.Application.Features.Districts.ViewModels;

public class DistrictViewModel : IMapFrom<Entity>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string ProvinceId { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, DistrictViewModel>();
    }
}
