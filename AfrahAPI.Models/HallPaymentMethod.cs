using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل طرق الدفع التي توفرها الصالة
/// </summary>
public class HallPaymentMethod
{
    /// <summary>
    /// المعرف الفريد لوسيلة دفع الصالة
    /// </summary>
    [Key]
    public Guid HallPaymentMethodID { get; set; }

    /// <summary>
    /// معرف الصالة
    /// </summary>
    [Required]
    public Guid HallID { get; set; }
    
    /// <summary>
    /// الصالة المرتبطة
    /// </summary>
    public Hall? Hall { get; set; }

    /// <summary>
    /// معرف وسيلة الدفع
    /// </summary>
    [Required]
    public Guid PaymentMethodID { get; set; }
    
    /// <summary>
    /// وسيلة الدفع المرتبطة
    /// </summary>
    public PaymentMethod? PaymentMethod { get; set; }

    /// <summary>
    /// هل وسيلة الدفع نشطة حالياً للصالة
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
