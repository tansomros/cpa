using Cpa.Application.Common.Interfaces;
using Cpa.Application.Features.ReferenceGroups.ViewModels;
using Cpa.Domain.Entities;

#pragma warning disable CS0618
namespace Cpa.Application.Features.ReferenceValues.ViewModels;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class ReferenceValueAbnormalViewModel : IMapFrom<ReferenceValue>
{
    public int Id { get; set; }

#pragma warning disable CS8618
    public required string ValueCode { get; set; }
    public required string Descriptions { get; set; }
    public int Sort { get; set; }

    public int ReferenceGroupId { get; set; }
    public ReferenceGroupViewModel ReferenceGroup { get; set; }

#pragma warning restore CS8618

    public bool? DeleteFlag { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public void Mapping(Profile profile)
    { 
        profile.CreateMap<ReferenceValue, ReferenceValueAbnormalViewModel>()           
               .ForMember(d => d.ValueCode, opt => opt.MapFrom<ValueResolver>())
               .ReverseMap();
    }
}

class ValueResolver : IValueResolver<ReferenceValue, ReferenceValueAbnormalViewModel, string>
{
    public string Resolve(ReferenceValue source, ReferenceValueAbnormalViewModel destination, string destMember, ResolutionContext context)
    {
        if (source.ValueCode.Equals("Abnormal"))
        {
            return "Y";
        }
        else if (source.ValueCode.Equals("Normal"))
        {
            return "N";
        }

        return "";
    }
}



[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class ReferenceValueAbnormalListViewModel
{
    public ICollection<ReferenceValueAbnormalViewModel> ReferenceValues { get; set; }

    public ReferenceValueAbnormalListViewModel()
    {
        ReferenceValues = new HashSet<ReferenceValueAbnormalViewModel>();
    }
}
