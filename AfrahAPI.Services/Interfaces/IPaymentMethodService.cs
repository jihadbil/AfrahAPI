using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.PaymentMethod;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة طرق الدفع
/// </summary>
public interface IPaymentMethodService : IBaseService<PaymentMethod, PaymentMethodCreateDTO, PaymentMethodReadDTO, PaymentMethodUpdateDTO>
{
    /// <summary>
    /// الحصول على جميع طرق الدفع النشطة
    /// </summary>
    /// <returns>قائمة بطرق الدفع النشطة</returns>
    Task<IEnumerable<PaymentMethodReadDTO>> GetActivePaymentMethodsAsync();
}
