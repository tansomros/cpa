using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.Role;

namespace BigLion.CPA.Application.Features.Roles.ViewModels;

public class RoleViewModel : IMapFrom<Entity>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Sort { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, RoleViewModel>();
    }
}
