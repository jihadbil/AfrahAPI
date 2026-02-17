using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمتل فواتير في نظام إدارة الحجوزات.
/// </summary>
public class Invoice
{

    /// <summary>
    /// معرف فريد لكل فاتورة.
    /// </summary>
 [Key]
 public Guid InvoiceId { get; set; }
    /// <summary>
    /// رقم الفاتورة.
    /// </summary>
    [Required(ErrorMessage = "رقم الفاتورة مطلوب")]
    [StringLength(50, ErrorMessage = "رقم الفاتورة يجب ألا يتجاوز 50 حرف")]
    public string InvoiceNumber { get; set; } = string.Empty;
   /// <summary>
   /// تاريخ إصدار الفاتورة.
   /// </summary>
   public DateTime invoiceDate { get; set; }

    /// <summary>
    /// اجمالي المبلغ في الفاتورة.
    /// </summary>
    public decimal    TotalAmount { get; set; }
    /// <summary>
    /// المبلغ المدفوع.
    /// </summary>
    public decimal PaidAmount { get; set; }
    /// <summary>
    /// المبلغ المتبقي.
    /// </summary>
    public decimal BalanceDue { get; set; }
    /// <summary>
    /// حالة الفاتورة (مثل: 'Paid', 'Unpaid', 'Partially Paid').
    /// </summary>
    [Required(ErrorMessage = "حالة الفاتورة مطلوبة")]
    [StringLength(50, ErrorMessage = "الحالة يجب ألا تتجاوز 50 حرف")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// تاريخ استحقاق الفاتورة.
    /// </summary>
    public DateTime DueDate { get; set; }
    /// <summary>
    /// تاريخ دفع الفاتورة.
    /// </summary>
    public DateTime? PaidAt { get; set; }
    /// <summary>
    /// هل الفاتورة مغلقة؟
    /// </summary>
    public bool IsLocked { get; set; }
    /// <summary>
    /// وصف الفاتورة.
    /// </summary>
    [StringLength(1000, ErrorMessage = "الوصف يجب ألا يتجاوز 1000 حرف")]
    public string? Description { get; set; }
    /// <summary>
    /// مبلغ العمولة.
    /// </summary>
    public decimal CommissionAmount { get; set; }
    /// <summary>
    /// نسبة العمولة.
    /// </summary>
    public decimal CommissionPercentage { get; set; }

    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Guid BookingId { get; set; }

    public Booking? Booking { get; set; }

    public ICollection<InvoiceItems> InvoiceItems { get; set; } = new List<InvoiceItems>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
