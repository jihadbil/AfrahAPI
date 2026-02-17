using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع فئات الصالات
/// </summary>
public class HallCategorieRepository : Repository<HallCategorie>, IHallCategorieRepository
{
    /// <summary>
    /// مُنشئ HallCategorieRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public HallCategorieRepository(ApplicationDbContext context) : base(context) { }

    /// <summary>
    /// جلب فئة مع جميع الصالات التابعة لها
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>الفئة مع صالاتها أو null إذا لم توجد</returns>
    public async Task<HallCategorie?> GetCategoryWithHallsAsync(Guid categoryId) =>
        await _dbSet.Include(hc => hc.Halls).FirstOrDefaultAsync(hc => hc.CategoryID == categoryId);

    /// <summary>
    /// جلب فئة بواسطة الاسم
    /// </summary>
    /// <param name="categoryName">اسم الفئة</param>
    /// <returns>الفئة أو null إذا لم توجد</returns>
    public async Task<HallCategorie?> GetCategoryByNameAsync(string categoryName) =>
        await _dbSet.FirstOrDefaultAsync(hc => hc.CategoryName == categoryName);

    /// <summary>
    /// البحث في الفئات بالاسم أو الوصف
    /// </summary>
    /// <param name="searchTerm">كلمة البحث</param>
    /// <returns>قائمة بالفئات المطابقة</returns>
    public async Task<IEnumerable<HallCategorie>> SearchCategoriesAsync(string searchTerm) =>
        await _dbSet.Where(hc => hc.CategoryName.Contains(searchTerm) || hc.Description.Contains(searchTerm)).ToListAsync();

    /// <summary>
    /// جلب الفئات التي تحتوي على صالات
    /// </summary>
    /// <returns>قائمة بالفئات النشطة التي لديها صالات</returns>
    public async Task<IEnumerable<HallCategorie>> GetCategoriesWithHallsAsync() =>
        await _dbSet.Include(hc => hc.Halls).Where(hc => hc.Halls.Any()).ToListAsync();

    /// <summary>
    /// عد عدد الصالات في فئة معينة
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>عدد الصالات التابعة للفئة</returns>
    public async Task<int> GetHallsCountByCategoryAsync(Guid categoryId) =>
        await _context.Halls.CountAsync(h => h.CategoryID == categoryId);
}
