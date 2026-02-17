using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Notification;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة الإشعارات
/// </summary>
public interface INotificationService : IBaseService<Notification, NotificationCreateDTO, NotificationReadDTO, NotificationUpdateDTO>
{
    /// <summary>
    /// الحصول على إشعارات مستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>قائمة بالإشعارات</returns>
    Task<IEnumerable<NotificationReadDTO>> GetUserNotificationsAsync(Guid userId);

    /// <summary>
    /// وضع علامة مقروء على إشعار
    /// </summary>
    /// <param name="notificationId">معرف الإشعار</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<NotificationReadDTO?> MarkAsReadAsync(Guid notificationId);

    /// <summary>
    /// إرسال تأكيد الحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>الإشعار المُنشأ</returns>
    Task<NotificationReadDTO> SendBookingConfirmationAsync(Guid bookingId);

    /// <summary>
    /// إرسال تأكيد الدفع
    /// </summary>
    /// <param name="paymentId">معرف الدفع</param>
    /// <returns>الإشعار المُنشأ</returns>
    Task<NotificationReadDTO> SendPaymentConfirmationAsync(Guid paymentId);

    /// <summary>
    /// إرسال تذكير بالحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>الإشعار المُنشأ</returns>
    Task<NotificationReadDTO> SendBookingReminderAsync(Guid bookingId);
}
