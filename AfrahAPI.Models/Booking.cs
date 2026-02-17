using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل  الحجز في نظام إدارة الحجوزات.
/// </summary>
public class Booking
{
    /// <summary>
    /// معرف فريد لكل حجز.
    /// </summary>
 [Key]
    public Guid     BookingId { get; set; }
   /// <summary>
   /// تاريخ بدء الحجز.
   /// </summary>
    [Required]
        public DateTime StartDate { get; set; }
    /// <summary>
    /// تاريخ انتهاء الحجز.
    /// </summary>
   [Required]
    public DateTime EndDate { get; set; }
    /// <summary>
    /// وقت بدء الحجز.
    /// </summary>
    public TimeSpan StartTime { get; set; }
    /// <summary>
    /// وقت انتهاء الحجز.
    /// </summary>
    public TimeSpan EndTime { get; set; }
    /// <summary>
    /// وضع التسعير للحجز.
    /// </summary>
     [Required(ErrorMessage = "وضع التسعير مطلوب")]
     [StringLength(50, ErrorMessage = "وضع التسعير يجب ألا يتجاوز 50 حرف")]
    public string PricingMode { get; set; } = string.Empty;
    /// <summary>
    /// السعر الإجمالي للحجز.
    /// </summary>
    public decimal TotalPrice { get; set; }
    /// <summary>
    /// مبلغ التأمين للحجز.
    /// </summary>
    public decimal DepositAmount { get; set; }
    /// <summary>
    /// تاريخ استحقاق مبلغ التأمين.
    /// </summary>
    public DateTime DepositDueDate { get; set; }
    /// <summary>
    /// هل تم دفع مبلغ التأمين؟
    /// </summary>
    public bool IsDepositPaid { get; set; }
    /// <summary>
    /// حالة الحجز.
    /// </summary>
    /// 
     [Required(ErrorMessage = "حالة الحجز مطلوبة")]
     [StringLength(50, ErrorMessage = "حالة الحجز يجب ألا تتجاوز 50 حرف")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    ///نوع المناسبة.
    /// </summary>
    [StringLength(100, ErrorMessage = "نوع المناسبة يجب ألا يتجاوز 100 حرف")]
    public string? EventType { get; set; }
    /// <summary>
    /// عدد الضيوف.
    /// </summary>
    public int NumberOfGuests { get; set; }
    /// <summary>
    /// ملاحظات الحجز.
    /// </summary>
    [StringLength(2000, ErrorMessage = "الملاحظات يجب ألا تتجاوز 2000 حرف")]
    public string? Notes { get; set; }
    /// <summary>
    /// نسبة الخصم.
    /// </summary>
    public decimal DiscountPercentage { get; set; }



    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

[Required]
 public Guid HallId { get; set; }

    public Hall? Hall { get; set; }

[Required]
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }  





    public ICollection<Invoice> Invoices { get; set; }  =new List<Invoice>();
}


