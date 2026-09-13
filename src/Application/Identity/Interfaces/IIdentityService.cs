using Cpa.Application.Identity.Commands;

namespace Cpa.Application.Identity.Interfaces;
public interface IIdentityService
{
    Task<LoginResponse> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken);
}
