using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع طرق الدفع المتاحة للصالات
/// </summary>
public interface IHallPaymentMethodRepository : IRepository<HallPaymentMethod>
{
    /// <summary>
    /// جلب جميع طرق الدفع المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بطرق الدفع المتاحة</returns>
    Task<IEnumerable<HallPaymentMethod>> GetPaymentMethodsByHallIdAsync(Guid hallId);

    /// <summary>
    /// التحقق من توفر طريقة دفع معينة لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>true إذا كانت متاحة، false خلاف ذلك</returns>
    Task<bool> IsPaymentMethodAvailableAsync(Guid hallId, Guid paymentMethodId);

    /// <summary>
    /// إضافة طريقة دفع لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    Task AddPaymentMethodToHallAsync(Guid hallId, Guid paymentMethodId);

    /// <summary>
    /// حذف طريقة دفع من صالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    Task RemovePaymentMethodFromHallAsync(Guid hallId, Guid paymentMethodId);

    /// <summary>
    /// جلب الصالات التي تدعم طريقة دفع معينة
    /// </summary>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>قائمة بطرق الدفع للصالات</returns>
    Task<IEnumerable<HallPaymentMethod>> GetHallsByPaymentMethodAsync(Guid paymentMethodId);
}
