using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع العملاء - توفر عمليات متخصصة للعملاء
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    /// <summary>
    /// جلب عميل مع جميع حجوزاته
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>العميل مع حجوزاته</returns>
    Task<Customer?> GetCustomerWithBookingsAsync(Guid customerId);

    /// <summary>
    /// جلب عميل مع تقييماته للصالات
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>العميل مع تقييماته</returns>
    Task<Customer?> GetCustomerWithRatingsAsync(Guid customerId);

    /// <summary>
    /// جلب العملاء حسب الدولة
    /// </summary>
    /// <param name="country">اسم الدولة</param>
    /// <returns>قائمة بالعملاء في الدولة المحددة</returns>
    Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country);

    /// <summary>
    /// جلب العملاء حسب المدينة
    /// </summary>
    /// <param name="city">اسم المدينة</param>
    /// <returns>قائمة بالعملاء في المدينة المحددة</returns>
    Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city);

    /// <summary>
    /// البحث عن العملاء بالاسم (الاسم الأول أو الأخير)
    /// </summary>
    /// <param name="name">الاسم المراد البحث عنه</param>
    /// <returns>قائمة بالعملاء المطابقين</returns>
    Task<IEnumerable<Customer>> SearchCustomersByNameAsync(string name);

    /// <summary>
    /// جلب عميل بواسطة معرف المستخدم (User ID)
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>العميل المرتبط بالمستخدم</returns>
    Task<Customer?> GetCustomerByUserIdAsync(Guid userId);

    /// <summary>
    /// جلب العملاء حسب الجنس
    /// </summary>
    /// <param name="gender">الجنس</param>
    /// <returns>قائمة بالعملاء حسب الجنس</returns>
    Task<IEnumerable<Customer>> GetCustomersByGenderAsync(string gender);
}
