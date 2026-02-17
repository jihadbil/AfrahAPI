using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Hall;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة الصالات
/// </summary>
public interface IHallService : IBaseService<Hall, HallCreateDTO, HallReadDTO, HallUpdateDTO>
{
    /// <summary>
    /// البحث عن الصالات مع تطبيق الفلاتر
    /// </summary>
    /// <param name="searchDto">معايير البحث</param>
    /// <returns>قائمة بالصالات المطابقة</returns>
    Task<IEnumerable<HallReadDTO>> SearchHallsAsync(HallSearchDTO searchDto);

    /// <summary>
    /// الحصول على الصالات المتاحة لنطاق تواريخ معين
    /// </summary>
    /// <param name="startDate">تاريخ البدء</param>
    /// <param name="endDate">تاريخ الانتهاء</param>
    /// <returns>قائمة بالصالات المتاحة</returns>
    Task<IEnumerable<HallReadDTO>> GetAvailableHallsAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// فحص توفر صالة معينة في نطاق تواريخ محدد
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="startDate">تاريخ البدء</param>
    /// <param name="endDate">تاريخ الانتهاء</param>
    /// <returns>true إذا كانت الصالة متاحة، false خلاف ذلك</returns>
    Task<bool> CheckHallAvailabilityAsync(Guid hallId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// الحصول على جميع صالات مالك معين
    /// </summary>
    /// <param name="ownerId">معرف صاحب الصالة</param>
    /// <returns>قائمة بصالات المالك</returns>
    Task<IEnumerable<HallReadDTO>> GetHallsByOwnerAsync(Guid ownerId);

    /// <summary>
    /// الحصول على الصالات حسب الفئة
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>قائمة بالصالات في هذه الفئة</returns>
    Task<IEnumerable<HallReadDTO>> GetHallsByCategoryAsync(Guid categoryId);

    /// <summary>
    /// التحقق من الصالة (للمسؤول فقط)
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>DTO القراءة للصالة المُحدثة</returns>
    Task<HallReadDTO?> VerifyHallAsync(Guid hallId);

    /// <summary>
    /// تبديل حالة توفر الصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="isAvailable">الحالة الجديدة</param>
    /// <returns>DTO القراءة للصالة المُحدثة</returns>
    Task<HallReadDTO?> UpdateHallAvailabilityAsync(Guid hallId, bool isAvailable);
}
