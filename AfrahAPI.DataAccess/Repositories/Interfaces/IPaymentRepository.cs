using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع المدفوعات - توفر عمليات متخصصة للمدفوعات
/// </summary>
public interface IPaymentRepository : IRepository<Payment>
{
    /// <summary>
    /// جلب جميع المدفوعات الخاصة بفاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>قائمة بالمدفوعات</returns>
    Task<IEnumerable<Payment>> GetPaymentsByInvoiceIdAsync(Guid invoiceId);

    /// <summary>
    /// جلب المدفوعات حسب وسيلة الدفع
    /// </summary>
    /// <param name="methodId">معرف وسيلة الدفع</param>
    /// <returns>قائمة بالمدفوعات</returns>
    Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(Guid methodId);

    /// <summary>
    /// جلب المدفوعات حسب الحالة
    /// </summary>
    /// <param name="status">حالة الدفع (Completed, Pending, Failed, etc.)</param>
    /// <returns>قائمة بالمدفوعات</returns>
    Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);

    /// <summary>
    /// جلب المدفوعات في فترة زمنية معينة
    /// </summary>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>قائمة بالمدفوعات</returns>
    Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// جلب دفعة بواسطة الرقم المرجعي
    /// </summary>
    /// <param name="referenceNumber">الرقم المرجعي</param>
    /// <returns>الدفعة</returns>
    Task<Payment?> GetPaymentByReferenceNumberAsync(string referenceNumber);

    /// <summary>
    /// حساب إجمالي المدفوعات لفاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>إجمالي المدفوعات</returns>
    Task<decimal> GetTotalPaymentsByInvoiceIdAsync(Guid invoiceId);

    /// <summary>
    /// حساب إجمالي المدفوعات في فترة زمنية
    /// </summary>
    /// <param name="startDate">تاريخ البداية</param>
    /// <param name="endDate">تاريخ النهاية</param>
    /// <returns>إجمالي المدفوعات</returns>
    Task<decimal> GetTotalPaymentsInRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// جلب المدفوعات الناجحة فقط
    /// </summary>
    /// <returns>قائمة بالمدفوعات الناجحة</returns>
    Task<IEnumerable<Payment>> GetSuccessfulPaymentsAsync();
}
