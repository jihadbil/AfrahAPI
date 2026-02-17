using System;

namespace AfrahAPI.Models.DTOs.Hall;

/// <summary>
/// DTO لعرض قائمة الصالات (نسخة مختصرة)
/// </summary>
public class HallListDTO
{
    /// <summary>
    /// معرف الصالة الفريد
    /// </summary>
    public Guid HallID { get; set; }

    /// <summary>
    /// اسم الصالة
    /// </summary>
    public string HallName { get; set; } = string.Empty;

    /// <summary>
    /// وصف مختصر للصالة
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// عنوان الصالة
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// سعة الصالة للضيوف
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// سعر إيجار الصالة بالساعة
    /// </summary>
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// سعر إيجار الصالة باليوم
    /// </summary>
    public decimal PricePerDay { get; set; }

    /// <summary>
    /// هل الصالة متاحة حالياً للحجز
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// هل تم التحقق من الصالة
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// رابط URL للصورة الرئيسية للصالة
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// معرف الفئة
    /// </summary>
    public Guid CategoryID { get; set; }
}
