namespace AfrahApp.Models.Auth;

public sealed class RegisterResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public List<string>? Errors { get; set; }
}
