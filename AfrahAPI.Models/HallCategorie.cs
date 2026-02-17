using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل فئات الصالات (أفراح، مناسبات، مؤتمرات، إلخ)
/// </summary>
public class HallCategorie
{
    /// <summary>
    /// معرف الفئة
    /// </summary>
    [Key]
    public Guid CategoryID { get; set; }
    /// <summary>
    /// اسم الفئة
    /// </summary>
    [Required(ErrorMessage = "اسم الفئة مطلوب")]
    [StringLength(100, ErrorMessage = "اسم الفئة يجب ألا يتجاوز 100 حرف")]
    public string CategoryName { get; set; } = string.Empty;
    /// <summary>
    /// وصف الفئة
    /// </summary>
    [Required(ErrorMessage = "الوصف مطلوب")]
    [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف")]
    public string Description { get; set; } = string.Empty;

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Hall> Halls { get; set; } = new List<Hall>();
}
