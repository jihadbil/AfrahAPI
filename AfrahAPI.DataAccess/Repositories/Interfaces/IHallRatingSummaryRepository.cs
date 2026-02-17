using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع ملخصات تقييمات الصالات
/// </summary>
public interface IHallRatingSummaryRepository : IRepository<HallRatingSummary>
{
    /// <summary>
    /// جلب ملخص التقييم بواسطة معرف الصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>ملخص التقييم</returns>
    Task<HallRatingSummary?> GetSummaryByHallIdAsync(Guid hallId);

    /// <summary>
    /// تحديث ملخص التقييم لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    Task UpdateSummaryAsync(Guid hallId);

    /// <summary>
    /// جلب الصالات الأعلى تقييماً
    /// </summary>
    /// <param name="topCount">عدد الصالات المطلوبة</param>
    /// <returns>قائمة بملخصات التقييمات</returns>
    Task<IEnumerable<HallRatingSummary>> GetTopRatedSummariesAsync(int topCount);
}
