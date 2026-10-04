using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.ServiceType;

namespace BigLion.CPA.Application.Features.ServiceTypes.ViewModels;

public class ServiceTypeViewModel : IMapFrom<Entity>
{
    public string ServiceTypeID { get; set; } = string.Empty;
    public string? ServiceName { get; set; }
    public string? Descriptions { get; set; }
    public string? Status { get; set; }
    public int? ProjectID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, ServiceTypeViewModel>();
    }
}
