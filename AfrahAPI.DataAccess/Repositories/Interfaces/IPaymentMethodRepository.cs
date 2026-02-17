using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع وسائل الدفع
/// </summary>
public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    /// <summary>
    /// جلب وسيلة دفع بواسطة الاسم
    /// </summary>
    /// <param name="methodName">اسم وسيلة الدفع</param>
    /// <returns>وسيلة الدفع</returns>
    Task<PaymentMethod?> GetPaymentMethodByNameAsync(string methodName);

    /// <summary>
    /// جلب جميع وسائل الدفع النشطة
    /// </summary>
    /// <returns>قائمة بوسائل الدفع النشطة</returns>
    Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync();

    /// <summary>
    /// جلب وسيلة دفع مع جميع الصالات التي تدعمها
    /// </summary>
    /// <param name="methodId">معرف وسيلة الدفع</param>
    /// <returns>وسيلة الدفع مع الصالات</returns>
    Task<PaymentMethod?> GetPaymentMethodWithHallsAsync(Guid methodId);
}
