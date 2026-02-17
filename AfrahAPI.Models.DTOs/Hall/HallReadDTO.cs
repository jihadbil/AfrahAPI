using System;

namespace AfrahAPI.Models.DTOs.Hall;

/// <summary>
/// DTO لقراءة بيانات الصالة
/// </summary>
public class HallReadDTO
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
    /// وصف الصالة
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// عنوان الصالة
    /// </summary>
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
    public int Capacity { get; set; }

    /// <summary>
    /// طريقة تسعير الصالة
    /// </summary>
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
    public bool IsAvailable { get; set; }

    /// <summary>
    /// هل تسمح الصالة بأكثر من حجز في نفس اليوم
    /// </summary>
    public bool AllowsMultipleReservationsPerDay { get; set; }

    /// <summary>
    /// هل يتم قبول الحجوزات تلقائياً أم تحتاج لموافقة المالك
    /// </summary>
    public bool AutoAcceptReservations { get; set; }

    /// <summary>
    /// هل تم التحقق من الصالة من قبل مدير التطبيق
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// رابط URL للصورة الرئيسية للصالة
    /// </summary>
    public string? MainImageUrl { get; set; }

    /// <summary>
    /// سياسة الإلغاء الخاصة بالصالة
    /// </summary>
    public string? CancellationPolicy { get; set; }

    /// <summary>
    /// نسبة العمولة الأساسية
    /// </summary>
    public decimal BaseCommissionRate { get; set; }

    /// <summary>
    /// وقت إضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// معرف صاحب الصالة
    /// </summary>
    public Guid OwnerUserID { get; set; }

    /// <summary>
    /// معرف الفئة
    /// </summary>
    public Guid CategoryID { get; set; }

    /// <summary>
    /// معرف ملخص التقييم
    /// </summary>
    public Guid? HallRatingSummaryID { get; set; }
}
