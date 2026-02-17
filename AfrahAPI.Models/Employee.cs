using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل الموظفين الدين يعملون بالصالات
/// </summary>
public class Employee
{
    /// <summary>
    /// معرف الموظف
    /// </summary>
   [Key]
    public Guid EmployeeId { get; set; }
    /// <summary>
    /// المسمى الوظيفي للموظف (مثال: 'مدير قاعة', 'محاسب', 'موظف استقبال').
    /// </summary>
    [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
    [StringLength(100, ErrorMessage = "المسمى الوظيفي يجب ألا يتجاوز 100 حرف")]
    public string JobTitle { get; set; } = string.Empty;
    /// <summary>
    /// تاريخ توظيف الموظف.
    /// </summary>
    public DateTime HireDate { get; set; }
    /// <summary>
    /// راتب الموظف (يمكن أن يكون حساسًا).
    /// </summary>
    public decimal Salary { get; set; }
    /// <summary>
    /// الاسم الأول للموظف.
    /// </summary>
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 50 حرف")]
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// اسم العائلة للموظف.
    /// </summary>
    [Required(ErrorMessage = "اسم العائلة مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم العائلة يجب أن يكون بين 2 و 50 حرف")]
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// رقم هاتف الموظف.
    /// </summary>
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// عنوان الموظف.
    /// </summary>
    [StringLength(300)]
    public string? Address { get; set; }
    /// <summary>
    /// تاريخ ميلاد الموظف.
    /// </summary>
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
    ///بلد الموظف
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
    public  double Longitude { get; set; }
    /// <summary>
    /// جنسية الموظف
    /// </summary>
    [StringLength(100)]
    public string? Nationality { get; set; }

    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

[Required]
    public Guid  HallID { get; set; }
    public Hall? Hall { get; set; }


   
}
