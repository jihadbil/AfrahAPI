using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Payment;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة المدفوعات
/// </summary>
public interface IPaymentService : IBaseService<Payment, PaymentCreateDTO, PaymentReadDTO, PaymentUpdateDTO>
{
    /// <summary>
    /// معالج دفعة جديدة
    /// </summary>
    /// <param name="createDto">بيانات الدفع</param>
    /// <returns>DTO القراءة للدفع المُنشأ</returns>
    Task<PaymentReadDTO> ProcessPaymentAsync(PaymentCreateDTO createDto);

    /// <summary>
    /// الحصول على مدفوعات فاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>قائمة بالمدفوعات</returns>
    Task<IEnumerable<PaymentReadDTO>> GetPaymentsByInvoiceAsync(Guid invoiceId);

    /// <summary>
    /// معالجة استرداد مبلغ
    /// </summary>
    /// <param name="paymentId">معرف الدفع</param>
    /// <param name="amount">المبلغ المسترد</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<PaymentReadDTO?> RefundPaymentAsync(Guid paymentId, decimal amount);
}
