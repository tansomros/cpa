using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemGroup;

namespace BigLion.CPA.Application.Features.DrugProblemGroups.ViewModels;

public class DrugProblemGroupViewModel : IMapFrom<Entity>
{
    public string Code { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, DrugProblemGroupViewModel>();
    }
}
