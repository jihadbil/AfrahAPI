using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل وسائل الدفع المتاحة
/// </summary>
public class PaymentMethod
{
    /// <summary>
    /// معرف وسيلة الدفع
    /// </summary>
    [Key]
    public Guid MethodId { get; set; }
    /// <summary>
    /// اسم وسيلة الدفع
    /// </summary>
    [Required(ErrorMessage = "اسم وسيلة الدفع مطلوب")]
    [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف")]
    public string MethodName { get; set; } = string.Empty;

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<HallPaymentMethod> HallPaymentMethods { get; set; } = new List<HallPaymentMethod>();
}
