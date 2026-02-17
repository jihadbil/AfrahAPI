using System;

namespace AfrahAPI.Models.DTOs.HallCategorie;

/// <summary>
/// DTO لقراءة بيانات فئة الصالة
/// </summary>
public class HallCategorieReadDTO
{
    /// <summary>
    /// معرف الفئة
    /// </summary>
    public Guid CategoryID { get; set; }

    /// <summary>
    /// اسم الفئة
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>
    /// وصف الفئة
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// وقت إضافة الحقل
    /// </summary>
 //   public DateTime CreatedAt { get; set; }

    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
