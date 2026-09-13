using System.Reflection;
using Cpa.Application.Exceptions;
using Cpa.Application.Common.Interfaces;
using Cpa.Application.Common.Security;

namespace Cpa.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationBehaviour(ICurrentUserService user)
    {
        _currentUserService = user;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        // No [Authorize] attributes — authorization not required
        if (!authorizeAttributes.Any())
        {
            return await next(cancellationToken);
        }

        // [Authorize(Policy = AllowAnonymous)] — skip all checks
        if (authorizeAttributes.Any(a => a.Policy == CpaPolicies.AllowAnonymous))
        {
            return await next(cancellationToken);
        }

        // [Authorize] present but user not authenticated
        if (string.IsNullOrEmpty(_currentUserService.Id))
        {
            throw new BigLionUnauthorizedAccessException("ไม่อนุญาตให้เข้าใช้งาน");
        }

        // Role-based authorization
        var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

        if (authorizeAttributesWithRoles.Any())
        {
            var authorized = false;

            foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
            {
                foreach (var role in roles)
                {
                    var isInRole = _currentUserService.IsInRole(role.Trim());
                    if (isInRole)
                    {
                        authorized = true;
                        break;
                    }
                }
            }

            // Must be a member of at least one role in roles
            if (!authorized)
            {
                throw new BigLionForbiddenAccessException("ไม่มีสิทธิ์ในการเข้าใช้งาน");
            }
        }

        // Policy-based authorization
        var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
        if (authorizeAttributesWithPolicies.Any())
        {
            foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
            {
                var authorized = await _currentUserService.IsInPolicyAsync(policy);

                if (!authorized)
                {
                    throw new BigLionForbiddenAccessException("ไม่มีสิทธิ์ในการเข้าใช้งาน");
                }
            }
        }

        // User is authorized
        return await next(cancellationToken);
    }
}
