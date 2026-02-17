using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallUnavailableDate;

public class HallUnavailableDateUpdateDTO
{
    [Required(ErrorMessage = "معرف التاريخ غير المتاح مطلوب")]
    public Guid UnavailableID { get; set; }

    [Required(ErrorMessage = "تاريخ البداية مطلوب")]
    public DateTime StartDateTime { get; set; }

    [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
    public DateTime EndDateTime { get; set; }

    [Required(ErrorMessage = "السبب مطلوب")]
    [StringLength(500, ErrorMessage = "السبب يجب ألا يتجاوز 500 حرف")]
    public string Reason { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "الملاحظات يجب ألا تتجاوز 1000 حرف")]
    public string? Notes { get; set; }

    public bool IsRecurring { get; set; }
    public DayOfWeek RecurringDayOfWeek { get; set; }
    public bool IsFullDay { get; set; }
    public bool IsActive { get; set; }
}
