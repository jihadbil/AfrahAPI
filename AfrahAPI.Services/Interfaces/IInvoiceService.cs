using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Invoice;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة الفواتير
/// </summary>
public interface IInvoiceService : IBaseService<Invoice, InvoiceCreateDTO, InvoiceReadDTO, InvoiceUpdateDTO>
{
    /// <summary>
    /// إنشاء فاتورة لحجز معين
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>DTO القراءة للفاتورة المُنشأة</returns>
    Task<InvoiceReadDTO> GenerateInvoiceForBookingAsync(Guid bookingId);

    /// <summary>
    /// الحصول على فواتير عميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بالفواتير</returns>
    Task<IEnumerable<InvoiceReadDTO>> GetInvoicesByCustomerAsync(Guid customerId);

    /// <summary>
    /// حساب إجمالي الفاتورة مع البنود
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>الإجمالي</returns>
    Task<decimal> CalculateInvoiceTotalAsync(Guid invoiceId);
}
