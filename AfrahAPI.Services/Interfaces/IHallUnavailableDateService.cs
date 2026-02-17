using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallUnavailableDate;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة التواريخ غير المتاحة للصالات
/// </summary>
public interface IHallUnavailableDateService : IBaseService<HallUnavailableDate, HallUnavailableDateCreateDTO, HallUnavailableDateReadDTO, HallUnavailableDateUpdateDTO>
{
    /// <summary>
    /// الحصول على التواريخ المحجوبة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتواريخ المحجوبة</returns>
    Task<IEnumerable<HallUnavailableDateReadDTO>> GetUnavailableDatesByHallIdAsync(Guid hallId);

    /// <summary>
    /// إضافة تاريخ محجوب لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="createDto">بيانات التاريخ المحجوب</param>
    /// <returns>DTO القراءة للتاريخ المُنشأ</returns>
    Task<HallUnavailableDateReadDTO> AddUnavailableDateAsync(Guid hallId, HallUnavailableDateCreateDTO createDto);

    /// <summary>
    /// إزالة تاريخ محجوب
    /// </summary>
    /// <param name="unavailableDateId">معرف التاريخ المحجوب</param>
    /// <returns>true إذا تم الحذف بنجاح</returns>
    Task<bool> RemoveUnavailableDateAsync(Guid unavailableDateId);
}
