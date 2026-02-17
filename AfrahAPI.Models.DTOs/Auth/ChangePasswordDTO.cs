using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Auth;

/// <summary>
/// DTO لتغيير كلمة المرور
/// </summary>
public class ChangePasswordDTO
{
    /// <summary>
    /// كلمة المرور الحالية
    /// </summary>
    [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// كلمة المرور الجديدة
    /// </summary>
    [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون 8 أحرف على الأقل")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// تأكيد كلمة المرور الجديدة
    /// </summary>
    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدها غير متطابقين")]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
