using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Invoice;

public class InvoiceUpdateDTO
{
    [Required(ErrorMessage = "معرف الفاتورة مطلوب")]
    public Guid InvoiceId { get; set; }

    [Required(ErrorMessage = "رقم الفاتورة مطلوب")]
    [StringLength(50, ErrorMessage = "رقم الفاتورة يجب ألا يتجاوز 50 حرف")]
    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime invoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceDue { get; set; }

    [Required(ErrorMessage = "حالة الفاتورة مطلوبة")]
    [StringLength(50, ErrorMessage = "الحالة يجب ألا تتجاوز 50 حرف")]
    public string Status { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    public bool IsLocked { get; set; }

    [StringLength(1000, ErrorMessage = "الوصف يجب ألا يتجاوز 1000 حرف")]
    public string? Description { get; set; }

    public decimal CommissionAmount { get; set; }
    public decimal CommissionPercentage { get; set; }
}
