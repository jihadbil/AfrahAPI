using System;

namespace AfrahAPI.Models.DTOs.Customer;

/// <summary>
/// DTO لقراءة بيانات العميل
/// </summary>
public class CustomerReadDTO
{
    /// <summary>
    /// معرف العميل
    /// </summary>
    public Guid CustomerID { get; set; }

    /// <summary>
    /// الاسم الأول للعميل
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الأخير للعميل
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// رقم الهاتف للعميل
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// عنوان العميل
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// تاريخ ميلاد العميل
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// صورة الملف الشخصي للعميل
    /// </summary>
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// جنس العميل
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// بلد العميل
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// المدينة التي يقيم فيها العميل
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// خط العرض للعميل
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// خط الطول للعميل
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// جنسية العميل
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// وقت إضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// معرف المستخدم المرتبط بالعميل
    /// </summary>
    public Guid UserID { get; set; }
}
