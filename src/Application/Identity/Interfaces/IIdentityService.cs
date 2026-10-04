using BigLion.CPA.Application.Features.Users.ViewModel;

namespace BigLion.CPA.Application.Identity.Interfaces;
public interface IIdentityService
{
    Task<UserViewModel> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken);
}
