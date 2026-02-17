using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع فئات الصالات - توفر عمليات متخصصة لفئات الصالات
/// </summary>
public interface IHallCategorieRepository : IRepository<HallCategorie>
{
    /// <summary>
    /// جلب فئة مع جميع الصالات التابعة لها
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>الفئة مع صالاتها</returns>
    Task<HallCategorie?> GetCategoryWithHallsAsync(Guid categoryId);

    /// <summary>
    /// جلب فئة بواسطة الاسم
    /// </summary>
    /// <param name="categoryName">اسم الفئة</param>
    /// <returns>الفئة</returns>
    Task<HallCategorie?> GetCategoryByNameAsync(string categoryName);

    /// <summary>
    /// البحث في الفئات بالاسم أو الوصف
    /// </summary>
    /// <param name="searchTerm">كلمة البحث</param>
    /// <returns>قائمة بالفئات المطابقة</returns>
    Task<IEnumerable<HallCategorie>> SearchCategoriesAsync(string searchTerm);

    /// <summary>
    /// جلب الفئات التي تحتوي على صالات
    /// </summary>
    /// <returns>قائمة بالفئات النشطة</returns>
    Task<IEnumerable<HallCategorie>> GetCategoriesWithHallsAsync();

    /// <summary>
    /// عد عدد الصالات في فئة معينة
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>عدد الصالات</returns>
    Task<int> GetHallsCountByCategoryAsync(Guid categoryId);
}
