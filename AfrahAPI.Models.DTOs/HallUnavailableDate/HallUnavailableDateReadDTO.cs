using System;

namespace AfrahAPI.Models.DTOs.HallUnavailableDate;

public class HallUnavailableDateReadDTO
{
    public Guid UnavailableID { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsRecurring { get; set; }
    public DayOfWeek RecurringDayOfWeek { get; set; }
    public bool IsFullDay { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallID { get; set; }
}
