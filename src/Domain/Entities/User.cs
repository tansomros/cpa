using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpa.Domain.Entities;
public class User : BaseEntity
{
    public string Username { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public string? PositionName { get; private set; }
    public DateTime? LastLog { get; private set; }
    public int RoleId { get; private set; }
    public virtual Role? Role { get; private set; }

    private User()
    {
        // EF Core
    }

    public User(
        string username,
        string passwordHash,
        string displayName,
        string? positionName,
        int roleId)
    {
        Username = username;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        PositionName = positionName;
        RoleId = roleId;

        IsActive = true;
        DeleteFlag = false;
        CreatedOn = DateTimeOffset.UtcNow;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        LastModified = DateTimeOffset.UtcNow;
    }

    public void UpdateLastLogin()
    {
        LastLog = DateTime.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
    }
}
