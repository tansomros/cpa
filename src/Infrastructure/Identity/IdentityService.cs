using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigLion.CPA.Application.Common.Exceptions;
using BigLion.CPA.Application.Features.Users.ViewModel;
using BigLion.CPA.Application.Identity.Interfaces;

namespace BigLion.CPA.Infrastructure.Identity;
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

    public async Task<UserViewModel> LoginAsync(
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

        return new UserViewModel
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            PositionName = user.PositionName,
            LastLog = user.LastLog,
            RoleId = user.RoleId,
            Role = user.Role?.Name ?? string.Empty,
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            DeleteFlag = user.DeleteFlag,
            IsActive = user.IsActive,
            CreatedOn = user.CreatedOn,
            LastModified = user.LastModified
        };
    }
}
