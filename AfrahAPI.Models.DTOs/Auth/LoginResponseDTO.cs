namespace AfrahAPI.Models.DTOs.Auth;

/// <summary>
/// DTO للرد على تسجيل الدخول
/// </summary>
public class LoginResponseDTO
{
    /// <summary>
    /// JWT Access Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// وقت انتهاء صلاحية Token (بالثواني)
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// معلومات المستخدم
    /// </summary>
    public UserInfoDTO UserInfo { get; set; } = new();
}

/// <summary>
/// معلومات المستخدم
/// </summary>
public class UserInfoDTO
{
    /// <summary>
    /// معرف المستخدم
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// اسم المستخدم
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// البريد الإلكتروني
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// الأدوار
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
