using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل بنود الفاتورة
/// </summary>
public class InvoiceItems
{
    /// <summary>
    /// معرف فريد لكل بند.
    /// </summary>
    [Key]
    public Guid ItemId { get; set; }
    /// <summary>
    /// اسم البند.
    /// </summary>
    [Required(ErrorMessage = "اسم البند مطلوب")]
    [StringLength(200, ErrorMessage = "اسم البند يجب ألا يتجاوز 200 حرف")]
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// نوع البند.
    /// </summary>
    [Required(ErrorMessage = "نوع البند مطلوب")]
    [StringLength(100, ErrorMessage = "نوع البند يجب ألا يتجاوز 100 حرف")]
    public string ItemType { get; set; } = string.Empty;
    /// <summary>
    /// وحدة القياس.
    /// </summary>
    [Required(ErrorMessage = "وحدة القياس مطلوبة")]
    [StringLength(50, ErrorMessage = "وحدة القياس يجب ألا تتجاوز 50 حرف")]
    public string UnitType { get; set; } = string.Empty;
    /// <summary>
    /// الكمية.
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// سعر الوحدة.
    /// </summary>
    public decimal UnitPrice { get; set; }
    /// <summary>
    /// المجموع الكلي للبند.
    /// </summary>
    public decimal Total { get; set; }
    /// <summary>
    /// ملاحظة إضافية عن البند.
    /// </summary>
    [StringLength(500, ErrorMessage = "الملاحظة يجب ألا تتجاوز 500 حرف")]
    public string? Note { get; set; }

    /// ///////////////حقول الوقت//////////

    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }

    public Guid? HallServiceId { get; set; }
    public HallServices? HallService { get; set; }
}
