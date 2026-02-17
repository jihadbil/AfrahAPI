using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل العملاء الدين يستخدمون النظام
/// </summary>
public class Customer
{
    /// <summary>
    /// معرف العميل
    /// </summary>
        [Key]
    public Guid CustomerID { get; set; }
    /// 
 
    /// <summary>
    /// الإسم الأول للعميل
    /// </summary>
 [Required(ErrorMessage = "الاسم الأول مطلوب")]
 [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 50 حرف")]
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// الإسم الأخير للعميل
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
    ///بلد العميل   
    /// </summary>
    [StringLength(100)]
    public string? Country { get; set; }
    /// <summary>
    ///المدينة التي يقيم فيها العميل
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


    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }


   /// </summary>
    /// معرف المستخدم المرتبط بالعميل   
    /// </summary>
   [Required]
    public Guid UserID { get; set; }

    public IdentityUser<Guid>? User { get; set; }

    public ICollection<Booking> Bookings { get; set; }=new List<Booking>();

public ICollection<ServiceRating> ServiceRatings { get; set; }=new List<ServiceRating>();

public ICollection<HallRating> HallRatings { get; set; }=new List<HallRating>();

























}
