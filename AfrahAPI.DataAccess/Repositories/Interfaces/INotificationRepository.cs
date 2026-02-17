using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع الإشعارات - توفر عمليات متخصصة للإشعارات
/// </summary>
public interface INotificationRepository : IRepository<Notification>
{
    /// <summary>
    /// جلب جميع الإشعارات الخاصة بمستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>قائمة بالإشعارات</returns>
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);

    /// <summary>
    /// جلب الإشعارات غير المقروءة لمستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>قائمة بالإشعارات غير المقروءة</returns>
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId);

    /// <summary>
    /// جلب الإشعارات المقروءة لمستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>قائمة بالإشعارات المقروءة</returns>
    Task<IEnumerable<Notification>> GetReadNotificationsAsync(Guid userId);

    /// <summary>
    /// جلب الإشعارات حسب النوع
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <param name="type">نوع الإشعار</param>
    /// <returns>قائمة بالإشعارات</returns>
    Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(Guid userId, string type);

    /// <summary>
    /// تحديث حالة الإشعار إلى مقروء
    /// </summary>
    /// <param name="notificationId">معرف الإشعار</param>
    Task MarkAsReadAsync(Guid notificationId);

    /// <summary>
    /// تحديث جميع الإشعارات إلى مقروءة لمستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    Task MarkAllAsReadAsync(Guid userId);

    /// <summary>
    /// عد الإشعارات غير المقروءة لمستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>عدد الإشعارات غير المقروءة</returns>
    Task<int> GetUnreadCountAsync(Guid userId);

    /// <summary>
    /// حذف الإشعارات القديمة (أقدم من تاريخ معين)
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <param name="olderThan">التاريخ المرجعي</param>
    Task DeleteOldNotificationsAsync(Guid userId, DateTime olderThan);

    /// <summary>
    /// جلب أحدث الإشعارات لمستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <param name="count">عدد الإشعارات المطلوبة</param>
    /// <returns>قائمة بأحدث الإشعارات</returns>
    Task<IEnumerable<Notification>> GetLatestNotificationsAsync(Guid userId, int count);
}
