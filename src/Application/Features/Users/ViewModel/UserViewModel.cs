using BigLion.CPA.Domain.Entities;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Application.Features.Users.ViewModel;
public class UserViewModel : IMapFrom<User>
{
    public int Id { get; set; }
#pragma warning disable CS8618
    public string Username { get; set; }  
    public string Password { get; set; }
    public string DisplayName { get; set; }
    public string? PositionName { get; set; }
    public DateTime? LastLog { get; set; }
    public int RoleId { get; set; }
    public bool? DeleteFlag { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
#pragma warning restore CS8618 
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserViewModel>(); 
    }

}

