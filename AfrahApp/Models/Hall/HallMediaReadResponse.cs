namespace AfrahApp.Models.Hall;

public sealed class HallMediaReadResponse
{
    public Guid MediaID { get; set; }
    public string MediaTitle { get; set; } = string.Empty;
    public string MediaPath { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HallID { get; set; }
}
