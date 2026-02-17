using System;

namespace AfrahAPI.Models.DTOs.ServiceRatingSummary;

public class ServiceRatingSummaryReadDTO
{
    public Guid ServiceRatingSummaryId { get; set; }
    public decimal RatingAverage { get; set; }
    public int TotalRatingsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallServiceID { get; set; }
}
