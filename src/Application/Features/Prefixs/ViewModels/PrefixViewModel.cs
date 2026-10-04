using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Prefix;

namespace BigLion.CPA.Application.Features.Prefixs.ViewModels;

public class PrefixViewModel : IMapFrom<Entity>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, PrefixViewModel>();
    }
}
