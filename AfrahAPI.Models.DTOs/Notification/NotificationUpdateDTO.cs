using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.Notification;

public class NotificationUpdateDTO
{
    [Required(ErrorMessage = "معرف الإشعار مطلوب")]
    public Guid NotificationID { get; set; }

    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}
