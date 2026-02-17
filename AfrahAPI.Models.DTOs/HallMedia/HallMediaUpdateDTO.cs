using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallMedia;

/// <summary>
/// DTO لتحديث وسائط صالة موجودة
/// </summary>
public class HallMediaUpdateDTO
{
    [Required(ErrorMessage = "معرف الوسائط مطلوب")]
    public Guid MediaID { get; set; }

    [Required(ErrorMessage = "عنوان الوسائط مطلوب")]
    [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف")]
    public string MediaTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "مسار الوسائط مطلوب")]
    [StringLength(500, ErrorMessage = "المسار يجب ألا يتجاوز 500 حرف")]
    public string MediaPath { get; set; } = string.Empty;

    [Required(ErrorMessage = "نوع الوسائط مطلوب")]
    [StringLength(50, ErrorMessage = "نوع الوسائط يجب ألا يتجاوز 50 حرف")]
    public string MediaType { get; set; } = string.Empty;

    [Required(ErrorMessage = "مسار الصورة المصغرة مطلوب")]
    [StringLength(500, ErrorMessage = "المسار يجب ألا يتجاوز 500 حرف")]
    public string ThumbnailPath { get; set; } = string.Empty;

    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    public string Caption { get; set; } = string.Empty;

    public bool IsMain { get; set; }
    public int DisplayOrder { get; set; }
}
