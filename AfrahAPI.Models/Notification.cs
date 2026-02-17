using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AfrahAPI.Models;
/// <summary>
/// يمثل الإشعارات المرسلة للمستخدمين في النظام
/// </summary>
public class Notification
{
    /// <summary>
    /// معرف فريد للإشعار
    /// </summary>
   [Key]
   public Guid NotificationID { get; set; }
   /// <summary>
   /// عنوان الإشعار
   /// </summary>
   [Required(ErrorMessage = "عنوان الإشعار مطلوب")]
   [StringLength(200, ErrorMessage = "العنوان يجب ألا يتجاوز 200 حرف")]
   public string Title { get; set; } = string.Empty;
   /// <summary>
   /// محتوى رسالة الإشعار
   /// </summary>
   [Required(ErrorMessage = "محتوى الإشعار مطلوب")]
   [StringLength(1000, ErrorMessage = "المحتوى يجب ألا يتجاوز 1000 حرف")]
   public string Message { get; set; } = string.Empty;
   /// <summary>
   /// نوع الإشعار (معلومة، تحذير، نجاح، خطأ)
   /// </summary>
   [Required(ErrorMessage = "نوع الإشعار مطلوب")]
   [StringLength(50, ErrorMessage = "النوع يجب ألا يتجاوز 50 حرف")]
   public string Type { get; set; } = string.Empty;
   /// <summary>
   /// الشاشة المستهدفة التي يجب التوجه إليها عند الضغط على الإشعار
   /// </summary>
   [Required(ErrorMessage = "الشاشة المستهدفة مطلوبة")]
   [StringLength(100, ErrorMessage = "الشاشة المستهدفة يجب ألا تتجاوز 100 حرف")]
   public string TargetScreen { get; set; } = string.Empty;
   /// <summary>
   /// معرف الكيان المرتبط بالإشعار (حجز، فاتورة، إلخ)
   /// </summary>
   public Guid RelatedEntityID { get; set; }
   /// <summary>
   /// هل تم قراءة الإشعار
   /// </summary>
   public bool IsRead { get; set; }
   /// <summary>
   /// تاريخ ووقت قراءة الإشعار
   /// </summary>
   public DateTime? ReadAt { get; set; }
   /// <summary>
   /// هل تم إرسال الإشعار بواسطة النظام تلقائياً
   /// </summary>
   public bool SentBySystem { get; set; }



    /// ///////////////حقول الوقت//////////


    /// <summary>
    ///وقت اضافة الحقل
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// وقت تحديث الحقل
    /// </summary>
    public DateTime? UpdatedAt { get; set; }




    public Guid UserID { get; set; }

    public IdentityUser<Guid>? User { get; set; }
}
