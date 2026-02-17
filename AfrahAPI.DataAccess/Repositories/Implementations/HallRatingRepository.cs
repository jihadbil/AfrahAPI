using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع تقييمات الصالات
/// </summary>
public class HallRatingRepository : Repository<HallRating>, IHallRatingRepository
{
    /// <summary>
    /// مُنشئ HallRatingRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallRatingRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// جلب جميع التقييمات لصالة معينة مع بيانات العملاء
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتقييمات مرتبة من الأحدث للأقدم</returns>
    public async Task<IEnumerable<HallRating>> GetRatingsByHallIdAsync(Guid hallId) =>
        await _dbSet.Where(hr => hr.HallID == hallId).Include(hr => hr.Customer).OrderByDescending(hr => hr.CreatedAt).ToListAsync();

    /// <summary>
    /// جلب جميع تقييمات عميل معين للصالات
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بتقييمات العميل</returns>
    public async Task<IEnumerable<HallRating>> GetRatingsByCustomerIdAsync(Guid customerId) =>
        await _dbSet.Where(hr => hr.CustomerID == customerId).Include(hr => hr.Hall).OrderByDescending(hr => hr.CreatedAt).ToListAsync();

    /// <summary>
    /// جلب تقييم مع جميع تفاصيله (العميل والصالة)
    /// </summary>
    /// <param name="ratingId">معرف التقييم</param>
    /// <returns>التقييم مع تفاصيله أو null</returns>
    public async Task<HallRating?> GetRatingWithDetailsAsync(Guid ratingId) =>
        await _dbSet.Include(hr => hr.Customer).Include(hr => hr.Hall).FirstOrDefaultAsync(hr => hr.RatingID == ratingId);

    /// <summary>
    /// جلب تقييمات صالة حسب درجة التقييم
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="rating">درجة التقييم</param>
    /// <returns>قائمة بالتقييمات المطابقة</returns>
    public async Task<IEnumerable<HallRating>> GetRatingsByScoreAsync(Guid hallId, int rating) =>
        await _dbSet.Where(hr => hr.HallID == hallId && hr.OverallRating == rating).Include(hr => hr.Customer).ToListAsync();

    /// <summary>
    /// حساب متوسط تقييم صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>متوسط التقييم</returns>
    public async Task<decimal> GetAverageRatingAsync(Guid hallId) =>
        await _dbSet.Where(hr => hr.HallID == hallId).AverageAsync(hr => (decimal)hr.OverallRating);

    /// <summary>
    /// جلب أحدث تقييمات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="count">عدد التقييمات المطلوبة</param>
    /// <returns>قائمة بأحدث التقييمات</returns>
    public async Task<IEnumerable<HallRating>> GetLatestRatingsAsync(Guid hallId, int count) =>
        await _dbSet.Where(hr => hr.HallID == hallId).Include(hr => hr.Customer).OrderByDescending(hr => hr.CreatedAt).Take(count).ToListAsync();
}
