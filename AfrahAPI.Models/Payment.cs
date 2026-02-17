using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل عمليات الدفع المتعلقة بالفواتير
/// </summary>
public class Payment
{

    /// <summary>
    /// معرف فريد لكل دفعة.
    /// </summary>
   [Key]
    public Guid PaymentId { get; set; }
    /// <summary>
    /// قيمة الدفعة.
    /// </summary>
    public decimal Amount { get; set; }
    /// <summary>
    /// تاريخ الدفع.
    /// </summary>
    public DateTime PaymentDate { get; set; }
    /// <summary>
    /// حالة الدفع (مثل: 'Success', 'Pending', 'Failed').
    /// </summary>
    [Required(ErrorMessage = "حالة الدفع مطلوبة")]
    [StringLength(50, ErrorMessage = "الحالة يجب ألا تتجاوز 50 حرف")]
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// الرقم المرجعي للدفع.
    /// </summary>
    [Required(ErrorMessage = "الرقم المرجعي مطلوب")]
    [StringLength(100, ErrorMessage = "الرقم المرجعي يجب ألا يتجاوز 100 حرف")]
    public string ReferenceNumber { get; set; } = string.Empty;
    /// <summary>
    /// ملاحظة عن الدفع.
    /// </summary>
    [StringLength(500, ErrorMessage = "الملاحظة يجب ألا تتجاوز 500 حرف")]
    public string? Note { get; set; }

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    public Guid MethodId { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
}
