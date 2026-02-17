using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع تقييمات الصالات - توفر عمليات متخصصة لتقييمات الصالات
/// </summary>
public interface IHallRatingRepository : IRepository<HallRating>
{
    /// <summary>
    /// جلب جميع التقييمات الخاصة بصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بتقييمات الصالة</returns>
    Task<IEnumerable<HallRating>> GetRatingsByHallIdAsync(Guid hallId);

    /// <summary>
    /// جلب جميع التقييمات الخاصة بعميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بتقييمات العميل</returns>
    Task<IEnumerable<HallRating>> GetRatingsByCustomerIdAsync(Guid customerId);

    /// <summary>
    /// جلب تقييم معين مع تفاصيل العميل والصالة
    /// </summary>
    /// <param name="ratingId">معرف التقييم</param>
    /// <returns>التقييم مع تفاصيله</returns>
    Task<HallRating?> GetRatingWithDetailsAsync(Guid ratingId);

    /// <summary>
    /// جلب التقييمات حسب درجة التقييم (1-5)
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="rating">درجة التقييم</param>
    /// <returns>قائمة بالتقييمات</returns>
    Task<IEnumerable<HallRating>> GetRatingsByScoreAsync(Guid hallId, int rating);

    /// <summary>
    /// حساب متوسط التقييم لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>متوسط التقييم</returns>
    Task<decimal> GetAverageRatingAsync(Guid hallId);

    /// <summary>
    /// جلب أحدث التقييمات لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="count">عدد التقييمات المطلوبة</param>
    /// <returns>قائمة بأحدث التقييمات</returns>
    Task<IEnumerable<HallRating>> GetLatestRatingsAsync(Guid hallId, int count);
}
