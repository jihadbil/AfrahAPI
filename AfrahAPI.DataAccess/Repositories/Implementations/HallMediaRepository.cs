using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع وسائط الصالات
/// </summary>
public class HallMediaRepository : Repository<HallMedia>, IHallMediaRepository
{
    /// <summary>
    /// مُنشئ HallMediaRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallMediaRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// جلب جميع الوسائط الخاصة بصالة معينة مرتبة حسب ترتيب العرض
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بوسائط الصالة</returns>
    public async Task<IEnumerable<HallMedia>> GetMediaByHallIdAsync(Guid hallId) =>
        await _dbSet.Where(hm => hm.HallID == hallId).OrderBy(hm => hm.DisplayOrder).ToListAsync();

    /// <summary>
    /// جلب وسائط صالة معينة حسب النوع (صورة/فيديو)
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="mediaType">نوع الوسائط</param>
    /// <returns>قائمة بالوسائط المطابقة</returns>
    public async Task<IEnumerable<HallMedia>> GetMediaByTypeAsync(Guid hallId, string mediaType) =>
        await _dbSet.Where(hm => hm.HallID == hallId && hm.MediaType == mediaType).OrderBy(hm => hm.DisplayOrder).ToListAsync();

    /// <summary>
    /// جلب الوسائط الرئيسية للصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>الوسائط الرئيسية أو null</returns>
    public async Task<HallMedia?> GetPrimaryMediaAsync(Guid hallId) =>
        await _dbSet.Where(hm => hm.HallID == hallId && hm.IsMain).FirstOrDefaultAsync();

    /// <summary>
    /// حذف جميع الوسائط الخاصة بصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    public async Task RemoveMediaByHallIdAsync(Guid hallId)
    {
        var media = await _dbSet.Where(hm => hm.HallID == hallId).ToListAsync();
        _dbSet.RemoveRange(media);
    }

    /// <summary>
    /// عد عدد الوسائط الخاصة بصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>عدد الوسائط</returns>
    public async Task<int> GetMediaCountByHallIdAsync(Guid hallId) =>
        await _dbSet.CountAsync(hm => hm.HallID == hallId);
}
