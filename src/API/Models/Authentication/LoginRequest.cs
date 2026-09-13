namespace Cpa.Presentation.API.Models.Authentication; 

public sealed class LoginRequest
{
    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
