using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع أصحاب الصالات - توفر عمليات متخصصة لأصحاب الصالات
/// </summary>
public interface IHallOwnerRepository : IRepository<HallOwner>
{
    /// <summary>
    /// جلب صاحب صالة مع جميع صالاته
    /// </summary>
    /// <param name="ownerId">معرف صاحب الصالة</param>
    /// <returns>صاحب الصالة مع صالاته</returns>
    Task<HallOwner?> GetOwnerWithHallsAsync(Guid ownerId);

    /// <summary>
    /// جلب أصحاب الصالات حسب الدولة
    /// </summary>
    /// <param name="country">اسم الدولة</param>
    /// <returns>قائمة بأصحاب الصالات في الدولة المحددة</returns>
    Task<IEnumerable<HallOwner>> GetOwnersByCountryAsync(string country);

    /// <summary>
    /// جلب أصحاب الصالات حسب المدينة
    /// </summary>
    /// <param name="city">اسم المدينة</param>
    /// <returns>قائمة بأصحاب الصالات في المدينة المحددة</returns>
    Task<IEnumerable<HallOwner>> GetOwnersByCityAsync(string city);

    /// <summary>
    /// البحث عن أصحاب الصالات بالاسم
    /// </summary>
    /// <param name="name">الاسم المراد البحث عنه</param>
    /// <returns>قائمة بأصحاب الصالات المطابقين</returns>
    Task<IEnumerable<HallOwner>> SearchOwnersByNameAsync(string name);

    /// <summary>
    /// جلب صاحب صالة بواسطة معرف المستخدم (User ID)
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>صاحب الصالة المرتبط بالمستخدم</returns>
    Task<HallOwner?> GetOwnerByUserIdAsync(Guid userId);

    /// <summary>
    /// جلب صاحب صالة بواسطة البريد الإلكتروني
    /// </summary>
    /// <param name="email">البريد الإلكتروني</param>
    /// <returns>صاحب الصالة</returns>
    Task<HallOwner?> GetOwnerByEmailAsync(string email);

    /// <summary>
    /// جلب أصحاب الصالات النشطين فقط
    /// </summary>
    /// <returns>قائمة بأصحاب الصالات النشطين</returns>
    Task<IEnumerable<HallOwner>> GetActiveOwnersAsync();
}
