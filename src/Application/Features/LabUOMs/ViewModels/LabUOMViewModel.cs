using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.LabUOM;

namespace BigLion.CPA.Application.Features.LabUOMs.ViewModels;

public class LabUOMViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public string? Descriptions { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, LabUOMViewModel>();
    }
}
