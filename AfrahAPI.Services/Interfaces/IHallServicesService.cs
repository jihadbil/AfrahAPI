using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallServices;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة خدمات الصالات
/// </summary>
public interface IHallServicesService : IBaseService<HallServices, HallServicesCreateDTO, HallServicesReadDTO, HallServicesUpdateDTO>
{
    /// <summary>
    /// الحصول على جميع خدمات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالخدمات</returns>
    Task<IEnumerable<HallServicesReadDTO>> GetServicesByHallIdAsync(Guid hallId);

    /// <summary>
    /// إضافة خدمة لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="createDto">بيانات الخدمة</param>
    /// <returns>DTO القراءة للخدمة المُنشأة</returns>
    Task<HallServicesReadDTO> AddServiceToHallAsync(Guid hallId, HallServicesCreateDTO createDto);
}
