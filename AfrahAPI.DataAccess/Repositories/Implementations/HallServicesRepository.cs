using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع خدمات الصالات
/// </summary>
public class HallServicesRepository : Repository<HallServices>, IHallServicesRepository
{
    /// <summary>
    /// مُنشئ HallServicesRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallServicesRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// جلب جميع الخدمات الخاصة بصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بخدمات الصالة</returns>
    public async Task<IEnumerable<HallServices>> GetServicesByHallIdAsync(Guid hallId) =>
        await _dbSet.Where(hs => hs.HallID ==  hallId).ToListAsync();

    /// <summary>
    /// جلب خدمة مع جميع تقييماتها وملخص التقييمات
    /// </summary>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <returns>الخدمة مع تقييماتها أو null</returns>
    public async Task<HallServices?> GetServiceWithRatingsAsync(Guid serviceId) =>
        await _dbSet.Include(hs => hs.ServiceRatings).Include(hs => hs.ServiceRatingSummary).FirstOrDefaultAsync(hs => hs.ServiceId == serviceId);

    /// <summary>
    /// جلب الخدمات المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالخدمات المتاحة</returns>
    public async Task<IEnumerable<HallServices>> GetAvailableServicesAsync(Guid hallId) =>
        await _dbSet.Where(hs => hs.HallID == hallId).ToListAsync();

    /// <summary>
    /// البحث في خدمات صالة معينة بالاسم أو الوصف
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="searchTerm">كلمة البحث</param>
    /// <returns>قائمة بالخدمات المطابقة</returns>
    public async Task<IEnumerable<HallServices>> SearchServicesAsync(Guid hallId, string searchTerm) =>
        await _dbSet.Where(hs => hs.HallID == hallId && (hs.ServiceName.Contains(searchTerm) || hs.Description.Contains(searchTerm))).ToListAsync();

    /// <summary>
    /// جلب خدمات صالة ضمن نطاق سعري محدد
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="minPrice">الحد الأدنى للسعر</param>
    /// <param name="maxPrice">الحد الأقصى للسعر</param>
    /// <returns>قائمة بالخدمات ضمن النطاق السعري</returns>
    public async Task<IEnumerable<HallServices>> GetServicesByPriceRangeAsync(Guid hallId, decimal minPrice, decimal maxPrice) =>
        await _dbSet.Where(hs => hs.HallID == hallId && hs.Price >= minPrice && hs.Price <= maxPrice).ToListAsync();

    /// <summary>
    /// عد عدد الخدمات المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>عدد الخدمات</returns>
    public async Task<int> GetServicesCountByHallIdAsync(Guid hallId) =>
        await _dbSet.CountAsync(hs => hs.HallID == hallId);

    /// <summary>
    /// جلب أفضل الخدمات تقييماً في صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="topCount">عدد الخدمات المطلوبة</param>
    /// <returns>قائمة بأفضل الخدمات تقييماً</returns>
    public async Task<IEnumerable<HallServices>> GetTopRatedServicesAsync(Guid hallId, int topCount) =>
        await _dbSet.Include(hs => hs.ServiceRatingSummary).Where(hs => hs.HallID == hallId && hs.ServiceRatingSummary != null)
            .OrderByDescending(hs => hs.ServiceRatingSummary!.RatingAverage).Take(topCount).ToListAsync();
}
