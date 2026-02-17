using System;

namespace AfrahAPI.Models.DTOs.Employee;

/// <summary>
/// DTO لقراءة بيانات الموظف
/// </summary>
public class EmployeeReadDTO
{
    /// <summary>
    /// معرف الموظف
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// المسمى الوظيفي للموظف
    /// </summary>
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ توظيف الموظف
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// راتب الموظف
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// الاسم الأول للموظف
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// اسم العائلة للموظف
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// رقم هاتف الموظف
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// عنوان الموظف
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// تاريخ ميلاد الموظف
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// صورة الملف الشخصي للموظف
    /// </summary>
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// جنس الموظف
    /// </summary>
    public string? Gender { get; set; }

    /// <summary>
    /// بلد الموظف
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// المدينة التي يقيم فيها الموظف
    /// </summary>
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
    /// معرف الصالة المرتبط بالموظف
    /// </summary>
    public Guid HallID { get; set; }
}
