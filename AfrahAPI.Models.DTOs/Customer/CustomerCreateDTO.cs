using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Customer;

/// <summary>
/// DTO لإنشاء عميل جديد
/// </summary>
public class CustomerCreateDTO
{
    /// <summary>
    /// الاسم الأول للعميل
    /// </summary>
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 50 حرف")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// الاسم الأخير للعميل
    /// </summary>
    [Required(ErrorMessage = "اسم العائلة مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم العائلة يجب أن يكون بين 2 و 50 حرف")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// رقم الهاتف للعميل
    /// </summary>
    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// عنوان العميل
    /// </summary>
    [StringLength(300)]
    public string? Address { get; set; }

    /// <summary>
    /// تاريخ ميلاد العميل
    /// </summary>
    [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// صورة الملف الشخصي للعميل
    /// </summary>
    [StringLength(500)]
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// جنس العميل
    /// </summary>
    [StringLength(20)]
    public string? Gender { get; set; }

    /// <summary>
    /// بلد العميل
    /// </summary>
    [StringLength(100)]
    public string? Country { get; set; }

    /// <summary>
    /// المدينة التي يقيم فيها العميل
    /// </summary>
    [StringLength(100)]
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
    [StringLength(100)]
    public string? Nationality { get; set; }

    /// <summary>
    /// معرف المستخدم المرتبط بالعميل
    /// </summary>
    [Required(ErrorMessage = "معرف المستخدم مطلوب")]
    public Guid UserID { get; set; }
}
