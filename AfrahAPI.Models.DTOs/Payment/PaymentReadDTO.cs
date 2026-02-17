using System;

namespace AfrahAPI.Models.DTOs.Payment;

public class PaymentReadDTO
{
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid MethodId { get; set; }
}
