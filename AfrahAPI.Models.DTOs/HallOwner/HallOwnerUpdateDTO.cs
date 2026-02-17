using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallOwner;

/// <summary>
/// DTO لتحديث بيانات صاحب صالة موجود
/// </summary>
public class HallOwnerUpdateDTO
{
    /// <summary>
    /// المعرف الفريد لصاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "معرف صاحب الصالة مطلوب")]
    public Guid OwnerID { get; set; }

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
    /// البريد الإلكتروني
    /// </summary>
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// عنوان صاحب الصالة
    /// </summary>
    [StringLength(300)]
    public string? Address { get; set; }

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

    /// <summary>
    /// صورة الملف الشخصي
    /// </summary>
    [StringLength(500)]
    public string? ProfileImageUrl { get; set; }
}
