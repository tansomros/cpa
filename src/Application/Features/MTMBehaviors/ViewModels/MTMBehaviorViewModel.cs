using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.MTMBehavior;

namespace BigLion.CPA.Application.Features.MTMBehaviors.ViewModels;

public class MTMBehaviorViewModel : IMapFrom<Entity>
{
    public int UID { get; set; }
    public int? MTMUID { get; set; }
    public int? ProblemUID { get; set; }
    public string? ProblemOther { get; set; }
    public string? Interventions { get; set; }
    public int? CUser { get; set; }
    public DateTime? CWhen { get; set; }
    public int? MUser { get; set; }
    public DateTime? MWhen { get; set; }
    public string? FinalResult { get; set; }
    public string? FinalResultOther { get; set; }
    public string? FatFollow { get; set; }
    public string? TasteFollow { get; set; }
    public string? ResultBegin { get; set; }
    public string? ResultEnd { get; set; }
    public string? Remark { get; set; }
    public string? isFollow { get; set; }
    public int? PatientID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, MTMBehaviorViewModel>();
    }
}
