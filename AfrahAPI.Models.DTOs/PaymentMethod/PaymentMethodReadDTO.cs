using System;

namespace AfrahAPI.Models.DTOs.PaymentMethod;

/// <summary>
/// DTO لقراءة بيانات وسيلة الدفع
/// </summary>
public class PaymentMethodReadDTO
{
    /// <summary>
    /// معرف وسيلة الدفع
    /// </summary>
    public Guid MethodId { get; set; }

    /// <summary>
    /// اسم وسيلة الدفع
    /// </summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// وقت إضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
