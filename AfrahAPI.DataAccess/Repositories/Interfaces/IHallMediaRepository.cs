using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع وسائط الصالات - توفر عمليات متخصصة لوسائط الصالات
/// </summary>
public interface IHallMediaRepository : IRepository<HallMedia>
{
    /// <summary>
    /// جلب جميع الوسائط الخاصة بصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بوسائط الصالة</returns>
    Task<IEnumerable<HallMedia>> GetMediaByHallIdAsync(Guid hallId);

    /// <summary>
    /// جلب وسائط الصالة حسب النوع (صورة، فيديو)
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="mediaType">نوع الوسائط (Image, Video)</param>
    /// <returns>قائمة بالوسائط من النوع المحدد</returns>
    Task<IEnumerable<HallMedia>> GetMediaByTypeAsync(Guid hallId, string mediaType);

    /// <summary>
    /// جلب الوسيط الرئيسي للصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>الوسيط الرئيسي</returns>
    Task<HallMedia?> GetPrimaryMediaAsync(Guid hallId);

    /// <summary>
    /// حذف جميع وسائط صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    Task RemoveMediaByHallIdAsync(Guid hallId);

    /// <summary>
    /// عد عدد الوسائط لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>عدد الوسائط</returns>
    Task<int> GetMediaCountByHallIdAsync(Guid hallId);
}
