using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع التواريخ غير المتاحة للصالات
/// </summary>
public interface IHallUnavailableDateRepository : IRepository<HallUnavailableDate>
{
    /// <summary>
    /// جلب جميع التواريخ غير المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتواريخ غير المتاحة</returns>
    Task<IEnumerable<HallUnavailableDate>> GetUnavailableDatesByHallIdAsync(Guid hallId);

    /// <summary>
    /// التحقق من توفر صالة في تاريخ معين
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="date">التاريخ المراد التحقق منه</param>
    /// <returns>true إذا كانت الصالة متاحة، false خلاف ذلك</returns>
    Task<bool> IsHallAvailableAsync(Guid hallId, DateTime date);

    /// <summary>
    /// التحقق من توفر صالة في فترة زمنية
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>true إذا كانت الصالة متاحة في كامل الفترة، false خلاف ذلك</returns>
    Task<bool> IsHallAvailableInRangeAsync(Guid hallId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// جلب التواريخ غير المتاحة في فترة زمنية معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>قائمة بالتواريخ غير المتاحة</returns>
    Task<IEnumerable<HallUnavailableDate>> GetUnavailableDatesInRangeAsync(Guid hallId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// حذف التواريخ غير المتاحة المنتهية
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    Task RemoveExpiredDatesAsync(Guid hallId);

    /// <summary>
    /// جلب التواريخ غير المتاحة المستقبلية فقط
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتواريخ غير المتاحة المستقبلية</returns>
    Task<IEnumerable<HallUnavailableDate>> GetFutureUnavailableDatesAsync(Guid hallId);
}
