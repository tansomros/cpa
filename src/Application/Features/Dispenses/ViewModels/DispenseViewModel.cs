using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Dispense;

namespace BigLion.CPA.Application.Features.Dispenses.ViewModels;

public class DispenseViewModel : IMapFrom<Entity>
{
    public long UID { get; set; }
    public string? RefID { get; set; }
    public DateOnly? RefillDate { get; set; }
    public int? PatientID { get; set; }
    public string? LocationID { get; set; }
    public string? Remark { get; set; }
    public int? MTMUID { get; set; }
    public int? DrugUID { get; set; }
    public string? TMTID { get; set; }
    public double? QTY { get; set; }
    public string? UOM { get; set; }
    public string? UsedRemark { get; set; }
    public string? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public string? MUser { get; set; }
    public DateTime? MWhen { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, DispenseViewModel>();
    }
}
