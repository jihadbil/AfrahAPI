using System;

namespace AfrahAPI.Models.DTOs.HallRatingSummary;

public class HallRatingSummaryReadDTO
{
    public Guid HallRatingSummaryId { get; set; }
    public decimal OverallRatingAverage { get; set; }
    public int TotalRatingsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallID { get; set; }
}
