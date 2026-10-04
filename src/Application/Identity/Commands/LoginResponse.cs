using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigLion.CPA.Application.Identity.Commands;
public sealed class LoginResponse
{
    public int UserId { get; init; }
    public string Username { get; init; } = default!;
    public string DisplayName { get; init; } = default!;
    public string PositinName { get; set; } = default!;
    public string AccessToken { get; init; } = default!;
    public DateTime ExpiresAt { get; init; }
    public string Role { get; init; } = string.Empty;
   
}
