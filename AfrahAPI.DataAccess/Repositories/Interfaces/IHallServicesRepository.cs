using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع خدمات الصالات - توفر عمليات متخصصة لخدمات الصالات
/// </summary>
public interface IHallServicesRepository : IRepository<HallServices>
{
    /// <summary>
    /// جلب جميع الخدمات الخاصة بصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بخدمات الصالة</returns>
    Task<IEnumerable<HallServices>> GetServicesByHallIdAsync(Guid hallId);

    /// <summary>
    /// جلب خدمة مع تقييماتها وملخص التقييمات
    /// </summary>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <returns>الخدمة مع تقييماتها</returns>
    Task<HallServices?> GetServiceWithRatingsAsync(Guid serviceId);

    /// <summary>
    /// جلب الخدمات المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالخدمات المتاحة</returns>
    Task<IEnumerable<HallServices>> GetAvailableServicesAsync(Guid hallId);

    /// <summary>
    /// البحث في الخدمات بالاسم أو الوصف
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="searchTerm">كلمة البحث</param>
    /// <returns>قائمة بالخدمات المطابقة</returns>
    Task<IEnumerable<HallServices>> SearchServicesAsync(Guid hallId, string searchTerm);

    /// <summary>
    /// جلب الخدمات حسب نطاق السعر
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="minPrice">الحد الأدنى للسعر</param>
    /// <param name="maxPrice">الحد الأقصى للسعر</param>
    /// <returns>قائمة بالخدمات في النطاق السعري</returns>
    Task<IEnumerable<HallServices>> GetServicesByPriceRangeAsync(Guid hallId, decimal minPrice, decimal maxPrice);

    /// <summary>
    /// عد عدد الخدمات لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>عدد الخدمات</returns>
    Task<int> GetServicesCountByHallIdAsync(Guid hallId);

    /// <summary>
    /// جلب الخدمات الأعلى تقييماً لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="topCount">عدد الخدمات المطلوبة</param>
    /// <returns>قائمة بأفضل الخدمات</returns>
    Task<IEnumerable<HallServices>> GetTopRatedServicesAsync(Guid hallId, int topCount);
}
