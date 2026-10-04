using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.PharmacyType;

namespace BigLion.CPA.Application.Features.PharmacyTypes.ViewModels;

public class PharmacyTypeViewModel : IMapFrom<Entity>
{
    public int Id { get; set; }
    public bool? DeleteFlag { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Sort { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, PharmacyTypeViewModel>();
    }
}
