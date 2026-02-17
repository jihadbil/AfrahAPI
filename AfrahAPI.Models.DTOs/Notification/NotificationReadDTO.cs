using System;

namespace AfrahAPI.Models.DTOs.Notification;

public class NotificationReadDTO
{
    public Guid NotificationID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string TargetScreen { get; set; } = string.Empty;
    public Guid RelatedEntityID { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public bool SentBySystem { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UserID { get; set; }
}
