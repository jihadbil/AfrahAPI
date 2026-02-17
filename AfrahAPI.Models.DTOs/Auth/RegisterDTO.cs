using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Auth;

/// <summary>
/// DTO لتسجيل مستخدم جديد
/// </summary>
public class RegisterDTO
{
    /// <summary>
    /// البريد الإلكتروني
    /// </summary>
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// اسم المستخدم
    /// </summary>
    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "اسم المستخدم يجب أن يكون بين 3 و 50 حرف")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// كلمة المرور
    /// </summary>
    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// تأكيد كلمة المرور
    /// </summary>
    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// رقم الهاتف
    /// </summary>
    [Phone(ErrorMessage = "رقم الهاتف غير صالح")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// الدور المطلوب للمستخدم (Customer, HallOwner, Employee)
    /// </summary>
    [Required(ErrorMessage = "الدور مطلوب")]
    public string Role { get; set; } = "Customer";
}
