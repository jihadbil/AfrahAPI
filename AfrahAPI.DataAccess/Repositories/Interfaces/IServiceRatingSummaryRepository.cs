using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع ملخصات تقييمات الخدمات
/// </summary>
public interface IServiceRatingSummaryRepository : IRepository<ServiceRatingSummary>
{
    /// <summary>
    /// جلب ملخص التقييم بواسطة معرف الخدمة
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    /// <returns>ملخص التقييم</returns>
    Task<ServiceRatingSummary?> GetSummaryByServiceIdAsync(Guid hallServiceId);

    /// <summary>
    /// تحديث ملخص التقييم لخدمة معينة
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    Task UpdateSummaryAsync(Guid hallServiceId);

    /// <summary>
    /// جلب الخدمات الأعلى تقييماً
    /// </summary>
    /// <param name="topCount">عدد الخدمات المطلوبة</param>
    /// <returns>قائمة بملخصات التقييمات</returns>
    Task<IEnumerable<ServiceRatingSummary>> GetTopRatedSummariesAsync(int topCount);
}
