using System.Security.Claims;

namespace Cpa.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? Id { get; }
        string? Name { get; }
        string? EmployeeId { get; }
        string? DoctorCode { get; }
        string? Position { get; }
        string? LoginName { get; }
        string? IdentityToken { get; }
        string? AccessToken { get; }
        List<Claim>? Claims { get; }
        bool? HasAdminRole { get; }
        bool? HasDoctorRole { get; }
        bool IsInRole(string role);
        Task<bool> IsInPolicyAsync(string policyName);
    }
}
