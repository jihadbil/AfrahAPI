namespace AfrahApp.Models.Hall;

public sealed class HallServiceReadResponse
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsIncluded { get; set; }
    public bool IsOptional { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallID { get; set; }
    public Guid? ServiceRatingSummaryID { get; set; }
}
