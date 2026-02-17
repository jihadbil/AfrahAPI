using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Employee;

/// <summary>
/// DTO لتحديث بيانات موظف موجود
/// </summary>
public class EmployeeUpdateDTO
{
    /// <summary>
    /// معرف الموظف
    /// </summary>
    [Required(ErrorMessage = "معرف الموظف مطلوب")]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// المسمى الوظيفي للموظف
    /// </summary>
    [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
    [StringLength(100, ErrorMessage = "المسمى الوظيفي يجب ألا يتجاوز 100 حرف")]
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ توظيف الموظف
    /// </summary>
    [Required(ErrorMessage = "تاريخ التوظيف مطلوب")]
    public DateTime HireDate { get; set; }

    /// <summary>
    /// راتب الموظف
    /// </summary>
    [Required(ErrorMessage = "الراتب مطلوب")]
    public decimal Salary { get; set; }

    /// <summary>
    /// الاسم الأول للموظف
    /// </summary>
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 50 حرف")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// اسم العائلة للموظف
    /// </summary>
    [Required(ErrorMessage = "اسم العائلة مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم العائلة يجب أن يكون بين 2 و 50 حرف")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// رقم هاتف الموظف
    /// </summary>
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// عنوان الموظف
    /// </summary>
    [StringLength(300)]
    public string? Address { get; set; }

    /// <summary>
    /// تاريخ ميلاد الموظف
    /// </summary>
    [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// صورة الملف الشخصي للموظف
    /// </summary>
    [StringLength(500)]
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// جنس الموظف
    /// </summary>
    [StringLength(20)]
    public string? Gender { get; set; }

    /// <summary>
    /// بلد الموظف
    /// </summary>
    [StringLength(100)]
    public string? Country { get; set; }

    /// <summary>
    /// المدينة التي يقيم فيها الموظف
    /// </summary>
    [StringLength(100)]
    public string? City { get; set; }

    /// <summary>
    /// خط العرض
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// خط الطول
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// جنسية الموظف
    /// </summary>
    [StringLength(100)]
    public string? Nationality { get; set; }

    /// <summary>
    /// معرف الصالة المرتبط بالموظف
    /// </summary>
    [Required(ErrorMessage = "معرف الصالة مطلوب")]
    public Guid HallID { get; set; }
}
