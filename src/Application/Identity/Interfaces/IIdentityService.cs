using BigLion.CPA.Application.Identity.Commands;

namespace BigLion.CPA.Application.Identity.Interfaces;
public interface IIdentityService
{
    Task<LoginResponse> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken);
}
