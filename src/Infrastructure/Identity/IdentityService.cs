using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cpa.Application.Common.Exceptions;
using Cpa.Application.Identity.Commands;
using Cpa.Application.Identity.Interfaces;

namespace Cpa.Infrastructure.Identity;
public sealed class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public IdentityService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByUsernameAsync(
            username,
            cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException();

        if (!_passwordHasher.Verify(user, user.PasswordHash, password))
        {
            throw new AuthenticationException(
                "Username or password is incorrect.");
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponse
        {
            UserId = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            PositinName = user.PositionName??"",
            Role = user.Role?.Name ?? string.Empty,
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(2)
        };
    }
}
