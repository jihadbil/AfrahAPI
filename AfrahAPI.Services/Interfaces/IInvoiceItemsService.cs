using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.InvoiceItems;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة بنود الفاتورة
/// </summary>
public interface IInvoiceItemsService : IBaseService<InvoiceItems, InvoiceItemsCreateDTO, InvoiceItemsReadDTO, InvoiceItemsUpdateDTO>
{
    /// <summary>
    /// الحصول على بنود فاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>قائمة بالبنود</returns>
    Task<IEnumerable<InvoiceItemsReadDTO>> GetItemsByInvoiceAsync(Guid invoiceId);

    /// <summary>
    /// إضافة بند لفاتورة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <param name="createDto">بيانات البند</param>
    /// <returns>DTO القراءة للبند المُنشأ</returns>
    Task<InvoiceItemsReadDTO> AddItemToInvoiceAsync(Guid invoiceId, InvoiceItemsCreateDTO createDto);

    /// <summary>
    /// حذف بند من فاتورة
    /// </summary>
    /// <param name="itemId">معرف البند</param>
    /// <returns>true إذا تم الحذف بنجاح</returns>
    Task<bool> RemoveItemAsync(Guid itemId);

    /// <summary>
    /// حساب إجمالي البند
    /// </summary>
    /// <param name="itemId">معرف البند</param>
    /// <returns>الإجمالي</returns>
    Task<decimal> CalculateItemTotalAsync(Guid itemId);

    /// <summary>
    /// حساب إجمالي جميع بنود الفاتورة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>الإجمالي الكلي</returns>
    Task<decimal> CalculateInvoiceItemsTotalAsync(Guid invoiceId);
}
