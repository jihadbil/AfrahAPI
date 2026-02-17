using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل ملفات الوسائط (صور/فيديو) الخاصة بالصالة
/// </summary>
public class HallMedia
{
    /// <summary>
    /// مُعرّف فريد لكل ملف وسائط.
    /// </summary>
 [Key]
    public Guid    MediaID { get; set; }
    /// <summary>
    /// عنوان الوسائط
    /// </summary>
    [Required(ErrorMessage = "عنوان الوسائط مطلوب")]
    [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف")]
    public string MediaTitle { get; set; } = string.Empty;
    /// <summary>
    /// مسار ملف الوسائط
    /// </summary>
    [Required(ErrorMessage = "مسار الوسائط مطلوب")]
    [StringLength(500, ErrorMessage = "المسار يجب ألا يتجاوز 500 حرف")]
    public string MediaPath { get; set; } = string.Empty;
    /// <summary>
    /// نوع الوسائط (صورة، فيديو)
    /// </summary>
    [Required(ErrorMessage = "نوع الوسائط مطلوب")]
    [StringLength(50, ErrorMessage = "نوع الوسائط يجب ألا يتجاوز 50 حرف")]
    public string MediaType { get; set; } = string.Empty;
    /// <summary>
    /// مسار صورة مصغرة
    /// </summary>
    [Required(ErrorMessage = "مسار الصورة المصغرة مطلوب")]
    [StringLength(500, ErrorMessage = "المسار يجب ألا يتجاوز 500 حرف")]
    public string ThumbnailPath { get; set; } = string.Empty;
    /// <summary>
    /// وصف الوسائط
    /// </summary>
    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    public string Caption { get; set; } = string.Empty;
    /// <summary>
    /// هل هذه الصورة الرئيسية
    /// </summary>
    public bool IsMain { get; set; }
    /// <summary>
    /// ترتيب العرض
    /// </summary>
    public int DisplayOrder { get; set; }

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Guid HallID { get; set; }
    public Hall? Hall { get; set; }
}
