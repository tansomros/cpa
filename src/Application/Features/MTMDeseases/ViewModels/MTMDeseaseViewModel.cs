using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMDesease;

namespace BigLion.CPA.Application.Features.MTMDeseases.ViewModels;

public class MTMDeseaseViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public string? ServiceTypeID { get; set; }
    public int? DeseaseUID { get; set; }
    public string? DeseaseOther { get; set; }
    public int? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public int? PatientID { get; set; }
    public string? ICDCode { get; set; }
    public string? DeseaseName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, MTMDeseaseViewModel>();
    }
}
