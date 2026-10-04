using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Pharmacist;

namespace BigLion.CPA.Application.Features.Pharmacists.ViewModels;

public class PharmacistViewModel : IMapFrom<Entity>
{
    public int Id { get; set; }
    public bool? DeleteFlag { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string? Name { get; set; }
    public string? LicenseNo { get; set; }
    public string? WorkTime { get; set; }
    public string? WorkType { get; set; }
    public string? PositionName { get; set; }
    public int? PharmacyId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, PharmacistViewModel>();
    }
}
