namespace AfrahApp.Models.Home;

public sealed class HomeHallCard
{
    public Guid HallId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string PriceText { get; set; } = string.Empty;
    public string PriceValueText { get; set; } = string.Empty;
    public string PriceHintText { get; set; } = "يبدأ من";
    public string RatingText { get; set; } = string.Empty;
    public int ReviewsCount { get; set; } = 120;
    public string CapacityText { get; set; } = string.Empty;
    public string TagText { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string AvailabilityBadgeText => IsAvailable ? "✓" : "✕";
    public string AvailabilityBadgeBackgroundColor => IsAvailable ? "#167A49" : "#8A6E77";
    public bool HasTag => !string.IsNullOrWhiteSpace(TagText);
}
