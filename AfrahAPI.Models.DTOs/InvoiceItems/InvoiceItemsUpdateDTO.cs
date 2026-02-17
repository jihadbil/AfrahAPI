using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.InvoiceItems;

public class InvoiceItemsUpdateDTO
{
    [Required(ErrorMessage = "معرف البند مطلوب")]
    public Guid ItemId { get; set; }

    [Required(ErrorMessage = "اسم البند مطلوب")]
    [StringLength(200, ErrorMessage = "اسم البند يجب ألا يتجاوز 200 حرف")]
    public string ItemName { get; set; } = string.Empty;

    [Required(ErrorMessage = "نوع البند مطلوب")]
    [StringLength(100, ErrorMessage = "نوع البند يجب ألا يتجاوز 100 حرف")]
    public string ItemType { get; set; } = string.Empty;

    [Required(ErrorMessage = "وحدة القياس مطلوبة")]
    [StringLength(50, ErrorMessage = "وحدة القياس يجب ألا تتجاوز 50 حرف")]
    public string UnitType { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }

    [StringLength(500, ErrorMessage = "الملاحظة يجب ألا تتجاوز 500 حرف")]
    public string? Note { get; set; }

    public Guid? HallServiceId { get; set; }
}
