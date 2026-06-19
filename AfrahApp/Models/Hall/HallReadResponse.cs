namespace AfrahApp.Models.Hall;

public sealed class HallReadResponse
{
    public Guid HallID { get; set; }
    public string HallName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? PricingMode { get; set; }
    public string? MainImageUrl { get; set; }
    public int Capacity { get; set; }
    public decimal PricePerHour { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal DefaultDepositAmount { get; set; }
    public bool IsAvailable { get; set; }
    public bool AllowsMultipleReservationsPerDay { get; set; }
    public bool AutoAcceptReservations { get; set; }
    public bool IsVerified { get; set; }
    public string? CancellationPolicy { get; set; }
    public decimal BaseCommissionRate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid OwnerUserID { get; set; }
    public Guid CategoryID { get; set; }
    public Guid? HallRatingSummaryID { get; set; }
}
