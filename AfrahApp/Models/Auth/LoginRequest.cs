namespace AfrahApp.Models.Auth;

public sealed class LoginRequest
{
    public string EmailOrUserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
