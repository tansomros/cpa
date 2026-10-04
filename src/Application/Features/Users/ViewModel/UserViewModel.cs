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
    public string? Email { get; set; }
    public int? PharmacyId { get; set; }
    public DateTime? LastLog { get; set; }
    public int RoleId { get; set; }
    public string? Role { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool? DeleteFlag { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? LastModified { get; set; }
#pragma warning restore CS8618 
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role == null ? null : src.Role.Name)); 
    }

}

