using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Notification;

public class NotificationCreateDTO
{
    [Required(ErrorMessage = "عنوان الإشعار مطلوب")]
    [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "محتوى الإشعار مطلوب")]
    [StringLength(1000, ErrorMessage = "المحتوى يجب ألا يتجاوز 1000 حرف")]
    public string Message { get; set; } = string.Empty;

    [Required(ErrorMessage = "نوع الإشعار مطلوب")]
    [StringLength(50, ErrorMessage = "النوع يجب ألا يتجاوز 50 حرف")]
    public string Type { get; set; } = string.Empty;

    [Required(ErrorMessage = "الشاشة المستهدفة مطلوبة")]
    [StringLength(100, ErrorMessage = "الشاشة المستهدفة يجب ألا تتجاوز 100 حرف")]
    public string TargetScreen { get; set; } = string.Empty;

    public Guid RelatedEntityID { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public bool SentBySystem { get; set; } = true;

    [Required(ErrorMessage = "معرف المستخدم مطلوب")]
    public Guid UserID { get; set; }
}
