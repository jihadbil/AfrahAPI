using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallPaymentMethod;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة طرق الدفع للصالات
/// </summary>
public interface IHallPaymentMethodService : IBaseService<HallPaymentMethod, HallPaymentMethodCreateDTO, HallPaymentMethodReadDTO, HallPaymentMethodUpdateDTO>
{
    /// <summary>
    /// إضافة طريقة دفع لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>DTO القراءة للربط المُنشأ</returns>
    Task<HallPaymentMethodReadDTO> AddPaymentMethodToHallAsync(Guid hallId, Guid paymentMethodId);

    /// <summary>
    /// إزالة طريقة دفع من صالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>true إذا تم الحذف بنجاح</returns>
    Task<bool> RemovePaymentMethodFromHallAsync(Guid hallId, Guid paymentMethodId);

    /// <summary>
    /// الحصول على طرق الدفع المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بطرق الدفع</returns>
    Task<IEnumerable<HallPaymentMethodReadDTO>> GetHallPaymentMethodsAsync(Guid hallId);

    /// <summary>
    /// الحصول على الصالات التي تدعم طريقة دفع معينة
    /// </summary>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>قائمة بالصالات</returns>
    Task<IEnumerable<HallPaymentMethodReadDTO>> GetHallsByPaymentMethodAsync(Guid paymentMethodId);
}
