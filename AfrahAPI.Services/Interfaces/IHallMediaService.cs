using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallMedia;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة وسائط الصالات
/// </summary>
public interface IHallMediaService : IBaseService<HallMedia, HallMediaCreateDTO, HallMediaReadDTO, HallMediaUpdateDTO>
{
    /// <summary>
    /// الحصول على جميع الوسائط لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالوسائط</returns>
    Task<IEnumerable<HallMediaReadDTO>> GetMediaByHallIdAsync(Guid hallId);

    /// <summary>
    /// رفع وسائط جديدة لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="createDto">بيانات الوسائط</param>
    /// <returns>DTO القراءة للوسائط المُنشأة</returns>
    Task<HallMediaReadDTO> UploadMediaAsync(Guid hallId, HallMediaCreateDTO createDto);

    /// <summary>
    /// حذف ملف وسائط
    /// </summary>
    /// <param name="mediaId">معرف الوسائط</param>
    /// <returns>true إذا تم الحذف بنجاح</returns>
    Task<bool> DeleteMediaAsync(Guid mediaId);
}
