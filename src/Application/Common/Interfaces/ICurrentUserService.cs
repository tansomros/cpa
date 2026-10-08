using System.Security.Claims;

namespace BigLion.CPA.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? Id { get; }
        string? Name { get; }
        string? EmployeeId { get; }
        string? Position { get; }
        string? LoginName { get; }
        string? IdentityToken { get; }
        string? AccessToken { get; }
        List<Claim>? Claims { get; }
        bool? HasAdminRole { get; }
        bool IsInRole(string role);
        Task<bool> IsInPolicyAsync(string policyName);
    }
}
