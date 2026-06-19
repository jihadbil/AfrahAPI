namespace AfrahApp.Models.Booking;

public sealed class BookingReadResponse
{
    public Guid BookingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public Guid HallId { get; set; }
    public Guid CustomerId { get; set; }
}
