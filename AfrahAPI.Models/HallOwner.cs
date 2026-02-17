using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace AfrahAPI.Models;
/// <summary>
/// يمتل اصحاب الصالات
/// </summary>
public class HallOwner    
{
    //////////// الحقول الأساسية//////////////////

    /// <summary>
    /// المعرف الفريد لصاحب الصالة
    /// </summary>
    [Key]
    public Guid OwnerID { get; set; }
    /// <summary>
    /// الأسم الأول لصاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "الاسم يجب أن يكون بين 2 و 50 حرف")]
    public string FirstName  { get; set; } = string.Empty;
    /// <summary>
    /// الإسم الأخير لصاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "اسم العائلة مطلوب")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم العائلة يجب أن يكون بين 2 و 50 حرف")]
    public string LastName  { get; set; } = string.Empty;
    /// <summary>
    /// رقم هاتف صاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [StringLength(20)]
    public string PhoneNumber  { get; set; } = string.Empty;
    /// <summary>
    /// البريد الألكتروني
    /// </summary>
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    [StringLength(100)]
    public string Email  { get; set; } = string.Empty;
    /// <summary>
    /// عنوان صاحب الصالة
    /// </summary>
    [StringLength(300)]
    public string? Address  { get; set; }
    /// <summary>
    /// جنس صاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "الجنس مطلوب")]
    [StringLength(20)]
    public string Gender  { get; set; } = string.Empty;
    /// <summary>
    /// تاريخ ميلاد صاحب الصالة
    /// </summary>
    [Required]
    public DateTime BirthDate { get; set; }
    /// <summary>
    /// جنسية صاحب الصالة
    /// </summary>
    [StringLength(100)]
    public string? Nationality  { get; set; }
    /// <summary>
    /// المدينة
    /// </summary>
    [StringLength(100)]
    public string? City  { get; set; }
    /// <summary>
    /// البلد
    /// </summary>
    [StringLength(100)]
    public string? Country  { get; set; }
    /// <summary>
    /// صورة الملف الشخصي
    /// </summary>
    [StringLength(500)]
    public string? ProfileImageUrl  { get; set; }

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
    public Guid UserID { get; set; }
    [ForeignKey("UserID")]
    public IdentityUser<Guid>? User { get; set; }

    public ICollection<Hall> Halls { get; set; } = new List<Hall>();
}
