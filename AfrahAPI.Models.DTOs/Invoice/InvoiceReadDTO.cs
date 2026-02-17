using System;

namespace AfrahAPI.Models.DTOs.Invoice;

public class InvoiceReadDTO
{
    public Guid InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime invoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal BalanceDue { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
    public bool IsLocked { get; set; }
    public string? Description { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal CommissionPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid BookingId { get; set; }
}
