using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Province;

namespace BigLion.CPA.Application.Features.Provinces.ViewModels;

public class ProvinceViewModel : IMapFrom<Entity>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEnglish { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string? ProvinceGroupId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, ProvinceViewModel>();
    }
}
