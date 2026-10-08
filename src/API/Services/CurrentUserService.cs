using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Presentation.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? EmployeeId { get; set; }

    public string? Position { get; set; }

    public string? LoginName { get; set; }

    public string? IdentityToken { get; set; }

    public string? AccessToken { get; set; }

    public List<Claim>? Claims { get; set; }

    public bool? HasAdminRole { get; set; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;

        Id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Name = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        EmployeeId = _httpContextAccessor.HttpContext?.User.FindFirstValue("EmployeeId");
        LoginName = _httpContextAccessor.HttpContext?.User.FindFirstValue("LoginName");
        Position = _httpContextAccessor.HttpContext?.User.FindFirstValue("Position");
        HasAdminRole = _httpContextAccessor.HttpContext?.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin") ?? false;
        Claims = _httpContextAccessor.HttpContext?.User.Claims.ToList();
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }

    public async Task<bool> IsInPolicyAsync(string policyName)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if(user == null)
        {
            return false;
        }

        var result = await _authorizationService.AuthorizeAsync(user, policyName);
        return result.Succeeded;
    }
}
