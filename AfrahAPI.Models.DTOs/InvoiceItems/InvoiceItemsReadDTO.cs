using System;

namespace AfrahAPI.Models.DTOs.InvoiceItems;

public class InvoiceItemsReadDTO
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public string UnitType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid? HallServiceId { get; set; }
}
