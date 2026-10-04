using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.LabItem;

namespace BigLion.CPA.Application.Features.LabItems.ViewModels;

public class LabItemViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public string? Name { get; set; }
    public string? AliasName { get; set; }
    public int? UOMUID { get; set; }
    public string? NormalRange { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, LabItemViewModel>();
    }
}
