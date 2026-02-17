using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallCategorie;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة فئات الصالات
/// </summary>
public interface IHallCategorieService : IBaseService<HallCategorie, HallCategorieCreateDTO, HallCategorieReadDTO, HallCategorieUpdateDTO>
{
    /// <summary>
    /// الحصول على الفئة مع جميع صالاتها
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>DTO القراءة للفئة</returns>
    Task<HallCategorieReadDTO?> GetCategoryWithHallsAsync(Guid categoryId);
}
