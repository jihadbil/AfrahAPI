namespace AfrahApp.Models.Booking;

public sealed class BookingCard
{
    public Guid BookingId { get; set; }
    public Guid HallId { get; set; }
    public DateTime StartDate { get; set; }
    public string StatusBucket { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public string HallLocation { get; set; } = string.Empty;
    public string HallImageUrl { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public string StatusIcon { get; set; } = string.Empty;
    public string StatusBackgroundColor { get; set; } = "#EEF2F6";
    public string StatusTextColor { get; set; } = "#617189";
    public string DateText { get; set; } = string.Empty;
    public string TimeText { get; set; } = string.Empty;
    public bool ShowTime { get; set; } = true;
    public string PriceText { get; set; } = string.Empty;
    public string PriceTextColor { get; set; } = "#E6195D";
    public bool ShowPrice { get; set; } = true;
    public string ActionText { get; set; } = "التفاصيل";
    public string ActionIcon { get; set; } = "‹";
    public string ActionTextColor { get; set; } = "#617189";
    public bool IsCancelled { get; set; }
    public bool IsMuted { get; set; }
}
