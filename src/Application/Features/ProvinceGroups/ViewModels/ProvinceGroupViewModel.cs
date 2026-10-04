using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.ProvinceGroup;

namespace BigLion.CPA.Application.Features.ProvinceGroups.ViewModels;

public class ProvinceGroupViewModel : IMapFrom<Entity>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, ProvinceGroupViewModel>();
    }
}
