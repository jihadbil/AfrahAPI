using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Hall;

/// <summary>
/// DTO للبحث عن الصالات مع الفلاتر
/// </summary>
public class HallSearchDTO
{
    /// <summary>
    /// الكلمة المفتاحية للبحث في اسم أو وصف الصالة
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// معرف الفئة للفلترة
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// المدينة للفلترة
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// الحد الأدنى للسعة
    /// </summary>
    public int? MinCapacity { get; set; }

    /// <summary>
    /// الحد الأقصى للسعة
    /// </summary>
    public int? MaxCapacity { get; set; }

    /// <summary>
    /// الحد الأدنى للسعر
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// الحد الأقصى للسعر
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// فقط الصالات المتاحة
    /// </summary>
    public bool? OnlyAvailable { get; set; }

    /// <summary>
    /// فقط الصالات المُحققة
    /// </summary>
    public bool? OnlyVerified { get; set; }

    /// <summary>
    /// تاريخ البدء للتحقق من التوفر
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// تاريخ الانتهاء للتحقق من التوفر
    /// </summary>
    public DateTime? EndDate { get; set; }
}
