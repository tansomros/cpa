using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.DrugMaster;

namespace BigLion.CPA.Application.Features.DrugMasters.ViewModels;

public class DrugMasterViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public string? TMTID { get; set; }
    public string? Name { get; set; }
    public string? AliasName { get; set; }
    public string? Manufacturer { get; set; }
    public string? FSN { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }
    public int? OUID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, DrugMasterViewModel>();
    }
}
