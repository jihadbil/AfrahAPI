namespace AfrahAPI.Models.DTOs.Auth;

/// <summary>
/// DTO للرد على عملية التسجيل
/// </summary>
public class RegisterResponseDTO
{
    /// <summary>
    /// هل نجحت العملية
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// رسالة النتيجة
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// معرف المستخدم المُنشأ
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// البريد الإلكتروني
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// الأخطاء في حال الفشل
    /// </summary>
    public List<string>? Errors { get; set; }
}
