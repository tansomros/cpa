using BigLion.CPA.Application.Common.Interfaces;
using Entity = BigLion.CPA.Domain.Entities.UserRoles;

namespace BigLion.CPA.Application.Features.UserRoleAssignments.ViewModels;

public class UserRolesViewModel : IMapFrom<Entity>
{
    public int RoleID { get; set; }
    public int UserID { get; set; }
    public int? isActive { get; set; }
    public string? UpdBy { get; set; }
    public DateTime? UpdDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Entity, UserRolesViewModel>();
    }
}
