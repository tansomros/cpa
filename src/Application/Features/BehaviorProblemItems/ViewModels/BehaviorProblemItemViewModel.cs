using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.BehaviorProblemItem;

namespace BigLion.CPA.Application.Features.BehaviorProblemItems.ViewModels;

public class BehaviorProblemItemViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public string? Descriptions { get; set; }
    public string? StatusFlag { get; set; }
    public int? Sort { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, BehaviorProblemItemViewModel>();
    }
}
