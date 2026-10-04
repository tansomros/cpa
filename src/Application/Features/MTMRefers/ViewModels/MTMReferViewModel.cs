using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMRefer;

namespace BigLion.CPA.Application.Features.MTMRefers.ViewModels;

public class MTMReferViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public string? HospitalType { get; set; }
    public string? HospitalName { get; set; }
    public int? PatientID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, MTMReferViewModel>();
    }
}
