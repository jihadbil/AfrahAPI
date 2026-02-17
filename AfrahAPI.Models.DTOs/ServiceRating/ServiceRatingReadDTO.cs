using System;

namespace AfrahAPI.Models.DTOs.ServiceRating;

public class ServiceRatingReadDTO
{
    public Guid ServiceRatingID { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallServiceID { get; set; }
    public Guid CustomerID { get; set; }
}
