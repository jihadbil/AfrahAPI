namespace AfrahApp.Models.Hall;

public sealed class HallRatingReadResponse
{
    public Guid RatingID { get; set; }
    public int OverallRating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallID { get; set; }
    public Guid CustomerID { get; set; }
}
