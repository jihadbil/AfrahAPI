using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع الحجوزات - توفر عمليات متخصصة للحجوزات
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    /// جلب حجز مع جميع تفاصيله (العميل، الصالة، الفواتير)
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>الحجز مع تفاصيله</returns>
    Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId);

    /// <summary>
    /// جلب جميع حجوزات عميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بالحجوزات</returns>
    Task<IEnumerable<Booking>> GetBookingsByCustomerAsync(Guid customerId);

    /// <summary>
    /// جلب جميع حجوزات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالحجوزات</returns>
    Task<IEnumerable<Booking>> GetBookingsByHallAsync(Guid hallId);

    /// <summary>
    /// جلب الحجوزات حسب الحالة
    /// </summary>
    /// <param name="status">حالة الحجز (Pending, Confirmed, Cancelled, etc.)</param>
    /// <returns>قائمة بالحجوزات</returns>
    Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status);

    /// <summary>
    /// جلب الحجوزات في فترة زمنية معينة
    /// </summary>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>قائمة بالحجوزات</returns>
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// جلب الحجوزات المعلقة (بانتظار التأكيد)
    /// </summary>
    /// <returns>قائمة بالحجوزات المعلقة</returns>
    Task<IEnumerable<Booking>> GetPendingBookingsAsync();

    /// <summary>
    /// جلب الحجوزات المؤكدة
    /// </summary>
    /// <returns>قائمة بالحجوزات المؤكدة</returns>
    Task<IEnumerable<Booking>> GetConfirmedBookingsAsync();

    /// <summary>
    /// جلب الحجوزات القادمة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالحجوزات القادمة</returns>
    Task<IEnumerable<Booking>> GetUpcomingBookingsAsync(Guid hallId);

    /// <summary>
    /// جلب حجوزات صالة في تاريخ معين
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="date">التاريخ</param>
    /// <returns>قائمة بالحجوزات</returns>
    Task<IEnumerable<Booking>> GetBookingsByDateAsync(Guid hallId, DateTime date);

    /// <summary>
    /// التحقق من وجود تعارض في الحجز
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>true إذا وجد تعارض، false خلاف ذلك</returns>
    Task<bool> HasConflictingBookingsAsync(Guid hallId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// جلب الحجوزات حسب نوع المناسبة
    /// </summary>
    /// <param name="eventType">نوع المناسبة</param>
    /// <returns>قائمة بالحجوزات</returns>
    Task<IEnumerable<Booking>> GetBookingsByEventTypeAsync(string eventType);

    /// <summary>
    /// حساب إجمالي الإيرادات من الحجوزات
    /// </summary>
    /// <param name="hallId">معرف الصالة (اختياري)</param>
    /// <param name="startDate">تاريخ البداية (اختياري)</param>
    /// <param name="endDate">تاريخ النهاية (اختياري)</param>
    /// <returns>إجمالي الإيرادات</returns>
    Task<decimal> GetTotalRevenueAsync(Guid? hallId = null, DateTime? startDate = null, DateTime? endDate = null);
}
