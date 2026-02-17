using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Hall;

/// <summary>
/// DTO لإنشاء صالة جديدة
/// </summary>
public class HallCreateDTO
{
    /// <summary>
    /// اسم الصالة
    /// </summary>
    [Required(ErrorMessage = "اسم الصالة مطلوب")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "اسم الصالة يجب أن يكون بين 3 و 100 حرف")]
    public string HallName { get; set; } = string.Empty;

    /// <summary>
    /// وصف الصالة
    /// </summary>
    [StringLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// عنوان الصالة
    /// </summary>
    [StringLength(300)]
    public string? Address { get; set; }

    /// <summary>
    /// احداثيات العرض للصالة
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// احداثيات الطول
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// سعة الصالة للضيوف
    /// </summary>
    [Required(ErrorMessage = "سعة الصالة مطلوبة")]
    public int Capacity { get; set; }

    /// <summary>
    /// طريقة تسعير الصالة
    /// </summary>
    [StringLength(50)]
    public string? PricingMode { get; set; }

    /// <summary>
    /// سعر إيجار الصالة بالساعة
    /// </summary>
    public decimal PricePerHour { get; set; }

    /// <summary>
    /// سعر إيجار الصالة باليوم
    /// </summary>
    public decimal PricePerDay { get; set; }

    /// <summary>
    /// مبلغ العربون الافتراضي المطلوب للحجز
    /// </summary>
    public decimal DefaultDepositAmount { get; set; }

    /// <summary>
    /// هل الصالة متاحة حالياً للحجز
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// هل تسمح الصالة بأكثر من حجز في نفس اليوم
    /// </summary>
    public bool AllowsMultipleReservationsPerDay { get; set; }

    /// <summary>
    /// هل يتم قبول الحجوزات تلقائياً
    /// </summary>
    public bool AutoAcceptReservations { get; set; }

    /// <summary>
    /// رابط URL للصورة الرئيسية للصالة
    /// </summary>
    [StringLength(500)]
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// سياسة الإلغاء الخاصة بالصالة
    /// </summary>
    [StringLength(1000)]
    public string? CancellationPolicy { get; set; }

    /// <summary>
    /// نسبة العمولة الأساسية
    /// </summary>
    public decimal BaseCommissionRate { get; set; }

    /// <summary>
    /// معرف صاحب الصالة
    /// </summary>
    [Required(ErrorMessage = "معرف صاحب الصالة مطلوب")]
    public Guid OwnerUserID { get; set; }

    /// <summary>
    /// معرف الفئة
    /// </summary>
    [Required(ErrorMessage = "معرف الفئة مطلوب")]
    public Guid CategoryID { get; set; }
}
