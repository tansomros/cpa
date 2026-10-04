using BigLion.CPA.Application.Common.Interfaces;
using BigLion.CPA.Domain.Entities;

#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceGroups.ViewModels;
[Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
public class ReferenceGroupViewModel : IMapFrom<ReferenceGroup>
{
    public int Id { get; set; }

#pragma warning disable CS8618
    public string Code { get; set; }
    public string Descriptions { get; set; }
    public int Sort { get; set; }
#pragma warning restore CS8618

    public bool? DeleteFlag { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReferenceGroup, ReferenceGroupViewModel>();
    }
}
