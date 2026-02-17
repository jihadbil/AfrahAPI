using System;

namespace AfrahAPI.Models.DTOs.Booking;

public class BookingReadDTO
{
    public Guid BookingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string PricingMode { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public decimal DepositAmount { get; set; }
    public DateTime DepositDueDate { get; set; }
    public bool IsDepositPaid { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? EventType { get; set; }
    public int NumberOfGuests { get; set; }
    public string? Notes { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallId { get; set; }
    public Guid CustomerId { get; set; }
}
