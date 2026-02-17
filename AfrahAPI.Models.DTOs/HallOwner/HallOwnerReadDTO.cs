using System;

namespace AfrahAPI.Models.DTOs.HallOwner;

/// <summary>
/// DTO لقراءة بيانات صاحب الصالة
/// </summary>
public class HallOwnerReadDTO
{
    /// <summary>
    /// المعرف الفريد لصاحب الصالة
    /// </summary>
    public Guid OwnerID { get; set; }

    /// <summary>
    /// الاسم الأول لصاحب الصالة
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الأخير لصاحب الصالة
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// رقم هاتف صاحب الصالة
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// البريد الإلكتروني
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// عنوان صاحب الصالة
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// جنس صاحب الصالة
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ ميلاد صاحب الصالة
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// جنسية صاحب الصالة
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// المدينة
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// البلد
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// صورة الملف الشخصي
    /// </summary>
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// وقت إضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// معرف المستخدم المرتبط
    /// </summary>
    public Guid UserID { get; set; }
}
