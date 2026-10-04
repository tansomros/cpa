using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.DrugProblemItem;

namespace BigLion.CPA.Application.Features.DrugProblemItems.ViewModels;

public class DrugProblemItemViewModel : IMapFrom<Entity>
{
    public string Code { get; set; } = string.Empty;
    public string? Descriptions { get; set; }
    public string? DrugProblemGroupUID { get; set; }
    public int? Sort { get; set; }
    public string? StatusFlag { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, DrugProblemItemViewModel>();
    }
}
