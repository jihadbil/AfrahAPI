using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل الصلات المسجلة بالنظام
/// </summary>
public class Hall
{
        /////////// الحقول الأساسية//////////////////

    /// <summary>
    /// معرف الصالة الفريد
    /// </summary>
    [Key]
    public Guid HallID { get; set; }

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
    public double   Latitude { get; set; }

/// <summary>
/// احداثيات الطول
/// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// سعة الصالة للضيوف
    /// </summary>
  public int  Capacity { get; set; }
    /// <summary>
    /// طريقة تسعير الصالة (باليوم، بالساعة، أو مخصص).
    /// </summary>
    [StringLength(50)]
    public string? PricingMode { get; set; }
    /// <summary>
    ///  سعر إيجار الصالة بالساعة (إذا كان PricingMode 'PerHour').
    /// </summary>
       public decimal  PricePerHour { get; set; }
    /// <summary>
    /// سعر إيجار الصالة باليوم (إذا كان PricingMode 'PerDay').
    /// </summary>
    public decimal PricePerDay { get; set; }
    /// <summary>
    ///مبلغ العربون الافتراضي المطلوب للحجز.
    /// </summary>
    public decimal DefaultDepositAmount { get; set; }
    /// <summary>
    ///  هل الصالة متاحة حالياً للحجز.
    /// </summary>
    public bool IsAvailable { get; set; }
    /// <summary>
    ///هل تسمح الصالة بأكثر من حجز في نفس اليوم (مثلاً: حجز صباحي ومسائي).
    /// </summary>
    public bool AllowsMultipleReservationsPerDay { get; set; }
    /// <summary>
    /// هل يتم قبول الحجوزات تلقائياً أم تحتاج لموافقة المالك.
    /// </summary>
    public bool AutoAcceptReservations { get; set; }
    /// <summary>
    /// هل تم التحقق من الصالة من قبل مدير التطبيق.
    /// </summary>
    public bool IsVerified { get; set; }
    /// <summary>
    /// رابط URL للصورة الرئيسية للصالة.
    /// </summary>
    [StringLength(500)]
    public string? MainImageUrl { get; set; }
    /// <summary>
    /// سياسة الإلغاء الخاصة بالصالة.
    /// </summary>
    [StringLength(1000)]
    public string? CancellationPolicy { get; set; }
    /// <summary>
    /// نسبة العمولة الأساسية التي يأخذها التطبيق من حجوزات هذه الصالة.
    /// </summary>
    public decimal BaseCommissionRate { get; set; }

    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    //////////// المفاتيح الخاريجية//////////////////

[Required]
    public Guid OwnerUserID { get; set; }

    public HallOwner? Owner { get; set; }

[Required]
   public Guid CategoryID { get; set; }
    public HallCategorie? Category { get; set; }

public Guid? HallRatingSummaryID { get; set; }
public HallRatingSummary? HallRatingSummary { get; set; }


    ////////////العلاقات/////////////
    public ICollection<HallMedia> HallMedia { get; set; }=new List<HallMedia>();

    public ICollection<HallServices> HallServices { get; set; }=new List<HallServices>();

    public ICollection<Employee> Employees { get; set; }=new List<Employee>();  

    public ICollection<HallRating> HallRatings { get; set; }=new List<HallRating>();



    public ICollection<Booking> Bookings { get; set; }=new List<Booking>();

    public ICollection<HallUnavailableDate> HallUnavailableDates { get; set; }=new List<HallUnavailableDate>();


    public ICollection<HallPaymentMethod> HallPaymentMethods { get; set; }=new List<HallPaymentMethod>();




}
