using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Running;

namespace BigLion.CPA.Application.Features.Runnings.ViewModels;

public class RunningViewModel : IMapFrom<Entity>
{
    public string Code { get; set; } = string.Empty;
    public int RefCode { get; set; }
    public int LastRunning { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, RunningViewModel>();
    }
}
