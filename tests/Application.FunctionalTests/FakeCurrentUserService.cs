using System.Security.Claims;
using BigLion.Application.Common.Interfaces;

namespace BigLion.Application.FunctionalTests;

public class FakeCurrentUserService : ICurrentUserService
{
    private readonly HashSet<string> _roles = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _policies = new(StringComparer.OrdinalIgnoreCase);

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? EmployeeId { get; set; }
    public string? DoctorCode { get; set; }
    public string? Position { get; set; }
    public string? LoginName { get; set; }
    public string? IdentityToken { get; set; }
    public string? AccessToken { get; set; }
    public List<Claim>? Claims { get; set; }
    public bool? HasAdminRole { get; set; }
    public bool? HasDoctorRole { get; set; }

    public bool IsInRole(string role) => _roles.Contains(role);

    public Task<bool> IsInPolicyAsync(string policyName) =>
        Task.FromResult(_policies.Contains(policyName));

    public void AddRoles(params string[] roles)
    {
        foreach (var role in roles)
            _roles.Add(role);
    }

    public void AddPolicies(params string[] policies)
    {
        foreach (var policy in policies)
            _policies.Add(policy);
    }

    public void Reset()
    {
        Id = null;
        Name = null;
        EmployeeId = null;
        DoctorCode = null;
        Position = null;
        LoginName = null;
        IdentityToken = null;
        AccessToken = null;
        Claims = null;
        HasAdminRole = null;
        HasDoctorRole = null;
        _roles.Clear();
        _policies.Clear();
    }
}
