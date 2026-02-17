using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع الفواتير - توفر عمليات متخصصة للفواتير
/// </summary>
public interface IInvoiceRepository : IRepository<Invoice>
{
    /// <summary>
    /// جلب فاتورة مع جميع بنودها ومدفوعاتها
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>الفاتورة مع تفاصيلها</returns>
    Task<Invoice?> GetInvoiceWithDetailsAsync(Guid invoiceId);

    /// <summary>
    /// جلب فاتورة بواسطة رقم الفاتورة
    /// </summary>
    /// <param name="invoiceNumber">رقم الفاتورة</param>
    /// <returns>الفاتورة</returns>
    Task<Invoice?> GetInvoiceByNumberAsync(string invoiceNumber);

    /// <summary>
    /// جلب جميع الفواتير الخاصة بحجز معين
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>قائمة بالفواتير</returns>
    Task<IEnumerable<Invoice>> GetInvoicesByBookingAsync(Guid bookingId);

    /// <summary>
    /// جلب الفواتير حسب الحالة
    /// </summary>
    /// <param name="status">حالة الفاتورة (Paid, Unpaid, Overdue, etc.)</param>
    /// <returns>قائمة بالفواتير</returns>
    Task<IEnumerable<Invoice>> GetInvoicesByStatusAsync(string status);

    /// <summary>
    /// جلب الفواتير غير المدفوعة
    /// </summary>
    /// <returns>قائمة بالفواتير غير المدفوعة</returns>
    Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync();

    /// <summary>
    /// جلب الفواتير المدفوعة جزئياً
    /// </summary>
    /// <returns>قائمة بالفواتير المدفوعة جزئياً</returns>
    Task<IEnumerable<Invoice>> GetPartiallyPaidInvoicesAsync();

    /// <summary>
    /// جلب الفواتير المتأخرة (تجاوزت تاريخ الاستحقاق)
    /// </summary>
    /// <returns>قائمة بالفواتير المتأخرة</returns>
    Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();

    /// <summary>
    /// جلب الفواتير في فترة زمنية معينة
    /// </summary>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>قائمة بالفواتير</returns>
    Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// حساب إجمالي المبلغ المستحق من الفواتير غير المدفوعة
    /// </summary>
    /// <returns>إجمالي المبلغ المستحق</returns>
    Task<decimal> GetTotalOutstandingAmountAsync();

    /// <summary>
    /// حساب إجمالي العمولة من الفواتير
    /// </summary>
    /// <param name="startDate">تاريخ البداية (اختياري)</param>
    /// <param name="endDate">تاريخ النهاية (اختياري)</param>
    /// <returns>إجمالي العمولة</returns>
    Task<decimal> GetTotalCommissionAsync(DateTime? startDate = null, DateTime? endDate = null);
}
