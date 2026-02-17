using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع بنود الفاتورة
/// </summary>
public interface IInvoiceItemsRepository : IRepository<InvoiceItems>
{
    /// <summary>
    /// جلب جميع البنود الخاصة بفاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>قائمة ببنود الفاتورة</returns>
    Task<IEnumerable<InvoiceItems>> GetItemsByInvoiceIdAsync(Guid invoiceId);

    /// <summary>
    /// جلب البنود الخاصة بخدمة معينة
    /// </summary>
    /// <param name="hallServiceId">معرف الخدمة</param>
    /// <returns>قائمة ببنود الفاتورة</returns>
    Task<IEnumerable<InvoiceItems>> GetItemsByServiceIdAsync(Guid hallServiceId);

    /// <summary>
    /// جلب البنود حسب النوع
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <param name="itemType">نوع البند</param>
    /// <returns>قائمة ببنود الفاتورة</returns>
    Task<IEnumerable<InvoiceItems>> GetItemsByTypeAsync(Guid invoiceId, string itemType);

    /// <summary>
    /// حساب إجمالي قيمة بنود فاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>إجمالي القيمة</returns>
    Task<decimal> GetTotalAmountByInvoiceIdAsync(Guid invoiceId);

    /// <summary>
    /// حذف جميع بنود فاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    Task RemoveItemsByInvoiceIdAsync(Guid invoiceId);
}
