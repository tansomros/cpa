using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigLion.CPA.Application.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BigLion.CPA.Infrastructure.Identity;

using BigLion.CPA.Domain.Entities;
using Microsoft.AspNetCore.Identity; 

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string Hash(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool Verify(User user, string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            hashedPassword,
            password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
