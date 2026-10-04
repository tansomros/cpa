using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.RunningConfig;

namespace BigLion.CPA.Application.Features.RunningConfigs.ViewModels;

public class RunningConfigViewModel : IMapFrom<Entity>
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCode { get; set; }
    public bool IsRef { get; set; }
    public int DigitCount { get; set; }
    public string? TemplateCode { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, RunningConfigViewModel>();
    }
}
