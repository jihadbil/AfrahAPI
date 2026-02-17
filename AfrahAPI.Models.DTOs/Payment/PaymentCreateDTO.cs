using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Payment;

public class PaymentCreateDTO
{
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "حالة الدفع مطلوبة")]
    [StringLength(50, ErrorMessage = "الحالة يجب ألا تتجاوز 50 حرف")]
    public string Status { get; set; } = string.Empty;

    [Required(ErrorMessage = "الرقم المرجعي مطلوب")]
    [StringLength(100, ErrorMessage = "الرقم المرجعي يجب ألا يتجاوز 100 حرف")]
    public string ReferenceNumber { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "الملاحظة يجب ألا تتجاوز 500 حرف")]
    public string? Note { get; set; }

    [Required(ErrorMessage = "معرف الفاتورة مطلوب")]
    public Guid InvoiceId { get; set; }

    [Required(ErrorMessage = "معرف طريقة الدفع مطلوب")]
    public Guid MethodId { get; set; }
}
