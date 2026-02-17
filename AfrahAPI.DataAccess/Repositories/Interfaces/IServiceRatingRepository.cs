using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع تقييمات الخدمات
/// </summary>
public interface IServiceRatingRepository : IRepository<ServiceRating>
{
    /// <summary>
    /// جلب جميع التقييمات الخاصة بخدمة معينة
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    /// <returns>قائمة بتقييمات الخدمة</returns>
    Task<IEnumerable<ServiceRating>> GetRatingsByServiceIdAsync(Guid hallServiceId);

    /// <summary>
    /// جلب جميع التقييمات الخاصة بعميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بتقييمات العميل</returns>
    Task<IEnumerable<ServiceRating>> GetRatingsByCustomerIdAsync(Guid customerId);

    /// <summary>
    /// جلب تقييم معين مع تفاصيل العميل والخدمة
    /// </summary>
    /// <param name="ratingId">معرف التقييم</param>
    /// <returns>التقييم مع تفاصيله</returns>
    Task<ServiceRating?> GetRatingWithDetailsAsync(Guid ratingId);

    /// <summary>
    /// حساب متوسط التقييم لخدمة معينة
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    /// <returns>متوسط التقييم</returns>
    Task<decimal> GetAverageRatingAsync(Guid hallServiceId);

    /// <summary>
    /// جلب أحدث التقييمات لخدمة معينة
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    /// <param name="count">عدد التقييمات المطلوبة</param>
    /// <returns>قائمة بأحدث التقييمات</returns>
    Task<IEnumerable<ServiceRating>> GetLatestRatingsAsync(Guid hallServiceId, int count);

    /// <summary>
    /// جلب التقييمات حسب درجة التقييم (1-5)
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    /// <param name="rating">درجة التقييم</param>
    /// <returns>قائمة بالتقييمات</returns>
    Task<IEnumerable<ServiceRating>> GetRatingsByScoreAsync(Guid hallServiceId, int rating);
}
