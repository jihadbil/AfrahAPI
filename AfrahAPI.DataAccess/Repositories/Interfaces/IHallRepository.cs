using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع الصالات - توفر عمليات متخصصة للصالات
/// </summary>
public interface IHallRepository : IRepository<Hall>
{
    /// <summary>
    /// جلب صالة مع جميع تفاصيلها (Media, Services, Ratings, etc.)
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>الصالة مع جميع تفاصيلها</returns>
    Task<Hall?> GetHallWithDetailsAsync(Guid hallId);

    /// <summary>
    /// جلب صالة مع وسائطها فقط
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>الصالة مع وسائطها</returns>
    Task<Hall?> GetHallWithMediaAsync(Guid hallId);

    /// <summary>
    /// جلب صالة مع خدماتها فقط
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>الصالة مع خدماتها</returns>
    Task<Hall?> GetHallWithServicesAsync(Guid hallId);

    /// <summary>
    /// جلب صالة مع تقييماتها وملخص التقييمات
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>الصالة مع تقييماتها</returns>
    Task<Hall?> GetHallWithRatingsAsync(Guid hallId);

    /// <summary>
    /// جلب الصالات حسب الفئة
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>قائمة بالصالات في الفئة المحددة</returns>
    Task<IEnumerable<Hall>> GetHallsByCategoryAsync(Guid categoryId);

    /// <summary>
    /// جلب الصالات حسب المالك
    /// </summary>
    /// <param name="ownerId">معرف المالك</param>
    /// <returns>قائمة بصالات المالك</returns>
    Task<IEnumerable<Hall>> GetHallsByOwnerAsync(Guid ownerId);

    /// <summary>
    /// جلب الصالات المتاحة في فترة زمنية معينة
    /// </summary>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>قائمة بالصالات المتاحة</returns>
    Task<IEnumerable<Hall>> GetAvailableHallsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// البحث في الصالات (بالاسم، الوصف، العنوان)
    /// </summary>
    /// <param name="searchTerm">كلمة البحث</param>
    /// <returns>قائمة بالصالات المطابقة</returns>
    Task<IEnumerable<Hall>> SearchHallsAsync(string searchTerm);

    /// <summary>
    /// جلب الصالات النشطة فقط
    /// </summary>
    /// <returns>قائمة بالصالات النشطة</returns>
    Task<IEnumerable<Hall>> GetActiveHallsAsync();

    /// <summary>
    /// جلب الصالات حسب نطاق السعر
    /// </summary>
    /// <param name="minPrice">الحد الأدنى للسعر</param>
    /// <param name="maxPrice">الحد الأقصى للسعر</param>
    /// <returns>قائمة بالصالات في النطاق السعري</returns>
    Task<IEnumerable<Hall>> GetHallsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

    /// <summary>
    /// جلب الصالات حسب السعة
    /// </summary>
    /// <param name="minCapacity">الحد الأدنى للسعة</param>
    /// <returns>قائمة بالصالات</returns>
    Task<IEnumerable<Hall>> GetHallsByCapacityAsync(int minCapacity);

    /// <summary>
    /// جلب الصالات الأعلى تقييماً
    /// </summary>
    /// <param name="topCount">عدد الصالات المطلوبة</param>
    /// <returns>قائمة بأفضل الصالات</returns>
    Task<IEnumerable<Hall>> GetTopRatedHallsAsync(int topCount);
}
