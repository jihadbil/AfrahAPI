using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع الإشعارات
/// </summary>
public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    /// <summary>
    /// مُنشئ NotificationRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(n => n.UserID == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId)
    {
        return await _dbSet
            .Where(n => n.UserID == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Notification>> GetReadNotificationsAsync(Guid userId)
    {
        return await _dbSet
            .Where(n => n.UserID == userId && n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(Guid userId, string type)
    {
        return await _dbSet
            .Where(n => n.UserID == userId && n.Type == type)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _dbSet.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            _dbSet.Update(notification);
        }
    }

    /// <inheritdoc/>
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var notifications = await _dbSet
            .Where(n => n.UserID == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        _dbSet.UpdateRange(notifications);
    }

    /// <inheritdoc/>
    public async Task<int> GetUnreadCountAsync(Guid userId)
    {
        return await _dbSet
            .CountAsync(n => n.UserID == userId && !n.IsRead);
    }

    /// <inheritdoc/>
    public async Task DeleteOldNotificationsAsync(Guid userId, DateTime olderThan)
    {
        var oldNotifications = await _dbSet
            .Where(n => n.UserID == userId && n.CreatedAt < olderThan)
            .ToListAsync();

        _dbSet.RemoveRange(oldNotifications);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Notification>> GetLatestNotificationsAsync(Guid userId, int count)
    {
        return await _dbSet
            .Where(n => n.UserID == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
