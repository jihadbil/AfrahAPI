using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Booking;

public class BookingUpdateDTO
{
    [Required(ErrorMessage = "معرف الحجز مطلوب")]
    public Guid BookingId { get; set; }

    [Required(ErrorMessage = "تاريخ البداية مطلوب")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
    public DateTime EndDate { get; set; }

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    [Required(ErrorMessage = "وضع التسعير مطلوب")]
    [StringLength(50, ErrorMessage = "وضع التسعير يجب ألا يتجاوز 50 حرف")]
    public string PricingMode { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; }
    public decimal DepositAmount { get; set; }
    public DateTime DepositDueDate { get; set; }
    public bool IsDepositPaid { get; set; }

    [Required(ErrorMessage = "حالة الحجز مطلوبة")]
    [StringLength(50, ErrorMessage = "حالة الحجز يجب ألا تتجاوز 50 حرف")]
    public string Status { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "نوع المناسبة يجب ألا يتجاوز 100 حرف")]
    public string? EventType { get; set; }

    public int NumberOfGuests { get; set; }

    [StringLength(2000, ErrorMessage = "الملاحظات يجب ألا تتجاوز 2000 حرف")]
    public string? Notes { get; set; }

    public decimal DiscountPercentage { get; set; }
}
