using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.PaymentMethod;

/// <summary>
/// DTO لإنشاء وسيلة دفع جديدة
/// </summary>
public class PaymentMethodCreateDTO
{
    /// <summary>
    /// اسم وسيلة الدفع
    /// </summary>
    [Required(ErrorMessage = "اسم وسيلة الدفع مطلوب")]
    [StringLength(100, ErrorMessage = "الاسم يجب ألا يتجاوز 100 حرف")]
    public string MethodName { get; set; } = string.Empty;
}
