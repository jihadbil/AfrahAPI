using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallOwner;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة أصحاب الصالات
/// </summary>
public interface IHallOwnerService : IBaseService<HallOwner, HallOwnerCreateDTO, HallOwnerReadDTO, HallOwnerUpdateDTO>
{
    /// <summary>
    /// الحصول على صاحب صالة بواسطة معرف المستخدم
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>DTO القراءة لصاحب الصالة أو null</returns>
    Task<HallOwnerReadDTO?> GetHallOwnerByUserIdAsync(Guid userId);

    /// <summary>
    /// الحصول على جميع الصالات المملوكة لهذا المالك
    /// </summary>
    /// <param name="ownerId">معرف صاحب الصالة</param>
    /// <returns>قائمة بالصالات</returns>
    Task<IEnumerable<object>> GetHallOwnerHallsAsync(Guid ownerId);

    /// <summary>
    /// تحديث ملف صاحب الصالة الشخصي
    /// </summary>
    /// <param name="ownerId">معرف صاحب الصالة</param>
    /// <param name="updateDto">بيانات التحديث</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<HallOwnerReadDTO?> UpdateProfileAsync(Guid ownerId, HallOwnerUpdateDTO updateDto);

    /// <summary>
    /// الحصول على إحصائيات صاحب الصالة (عدد الصالات، الحجوزات، الإيرادات)
    /// </summary>
    /// <param name="ownerId">معرف صاحب الصالة</param>
    /// <returns>كائن إحصائيات</returns>
    Task<object> GetOwnerStatisticsAsync(Guid ownerId);
}
