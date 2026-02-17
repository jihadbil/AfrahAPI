using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل التواريخ والأوقات غير المتاحة للحجز في الصالة
/// </summary>
public class HallUnavailableDate
{

    /// <summary>
    /// معرف فريد للتاريخ غير المتاح
    /// </summary>
    [Key]
    public Guid UnavailableID { get; set; }
    /// <summary>
    /// تاريخ ووقت بداية فترة عدم التوفر
    /// </summary>
    [Required(ErrorMessage = "تاريخ البداية مطلوب")]
    public DateTime StartDateTime { get; set; }
   /// <summary>
   /// تاريخ ووقت نهاية فترة عدم التوفر
   /// </summary>
   [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
    public DateTime EndDateTime { get; set; }
    /// <summary>
    /// سبب عدم توفر الصالة (صيانة، حجز خاص، إلخ)
    /// </summary>
    [Required(ErrorMessage = "السبب مطلوب")]
    [StringLength(500, ErrorMessage = "السبب يجب ألا يتجاوز 500 حرف")]
    public string Reason { get; set; } = string.Empty;
    /// <summary>
    /// ملاحظات إضافية حول فترة عدم التوفر
    /// </summary>
    [StringLength(1000, ErrorMessage = "الملاحظات يجب ألا تتجاوز 1000 حرف")]
    public string? Notes { get; set; }
    /// <summary>
    /// هل هذا التاريخ متكرر (مثلاً: كل يوم جمعة)
    /// </summary>
    public bool IsRecurring { get; set; }
    /// <summary>
    /// في حالة التكرار، ما هو اليوم المتكرر من الأسبوع
    /// </summary>
    public DayOfWeek RecurringDayOfWeek { get; set; }
    /// <summary>
    /// هل عدم التوفر يشمل اليوم كاملاً أم فترة محددة
    /// </summary>
    public bool IsFullDay { get; set; }
    /// <summary>
    /// هل هذا التاريخ نشط حالياً
    /// </summary>
    public bool IsActive { get; set; }


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
