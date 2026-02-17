using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallOwner;

/// <summary>
/// DTO لتسجيل صاحب صالة جديد (يتضمن بيانات المستخدم وصاحب الصالة)
/// </summary>
public class HallOwnerRegisterDTO
{
    // بيانات المستخدم
    /// <summary>
    /// البريد الإلكتروني
    /// </summary>
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// كلمة المرور
    /// </summary>
    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// تأكيد كلمة المرور
    /// </summary>
    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقين")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // بيانات صاحب الصالة
    /// <summary>
    /// الاسم الأول لصاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 50 حرف")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الأخير لصاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "اسم العائلة مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم العائلة يجب أن يكون بين 2 و 50 حرف")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// رقم هاتف صاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// جنس صاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "الجنس مطلوب")]
    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ ميلاد صاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// عنوان صاحب الصالة
    /// </summary>
    [StringLength(300)]
    public string? Address { get; set; }

    /// <summary>
    /// جنسية صاحب الصالة
    /// </summary>
    [StringLength(100)]
    public string? Nationality { get; set; }

    /// <summary>
    /// المدينة
    /// </summary>
    [StringLength(100)]
    public string? City { get; set; }

    /// <summary>
    /// البلد
    /// </summary>
    [StringLength(100)]
    public string? Country { get; set; }
}
