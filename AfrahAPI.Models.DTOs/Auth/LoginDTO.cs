using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Auth;

/// <summary>
/// DTO لتسجيل الدخول
/// </summary>
public class LoginDTO
{
    /// <summary>
    /// البريد الإلكتروني أو اسم المستخدم
    /// </summary>
    [Required(ErrorMessage = "البريد الإلكتروني أو اسم المستخدم مطلوب")]
    public string EmailOrUserName { get; set; } = string.Empty;

    /// <summary>
    /// كلمة المرور
    /// </summary>
    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
