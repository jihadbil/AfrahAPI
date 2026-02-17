using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.PaymentMethod;

/// <summary>
/// DTO لتحديث بيانات وسيلة دفع موجودة
/// </summary>
public class PaymentMethodUpdateDTO
{
    /// <summary>
    /// معرف وسيلة الدفع
    /// </summary>
    [Required(ErrorMessage = "معرف وسيلة الدفع مطلوب")]
    public Guid MethodId { get; set; }

    /// <summary>
    /// اسم وسيلة الدفع
    /// </summary>
    [Required(ErrorMessage = "اسم وسيلة الدفع مطلوب")]
    [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف")]
    public string MethodName { get; set; } = string.Empty;
}
