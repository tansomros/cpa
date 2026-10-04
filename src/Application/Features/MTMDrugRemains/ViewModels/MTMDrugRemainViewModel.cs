using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMDrugRemain;

namespace BigLion.CPA.Application.Features.MTMDrugRemains.ViewModels;

public class MTMDrugRemainViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public string? ServiceTypeID { get; set; }
    public int? DrugUID { get; set; }
    public double? QTY { get; set; }
    public string? UOM { get; set; }
    public string? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public string? MUser { get; set; }
    public DateTime? MWhen { get; set; }
    public int? PatientID { get; set; }
    public string? RemainFrom { get; set; }
    public int? ReasonUID { get; set; }
    public string? ReasonRemark { get; set; }
    public string? TMTID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, MTMDrugRemainViewModel>();
    }
}
