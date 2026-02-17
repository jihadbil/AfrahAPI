using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.ServiceRating;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة تقييمات الخدمات
/// </summary>
public interface IServiceRatingService : IBaseService<ServiceRating, ServiceRatingCreateDTO, ServiceRatingReadDTO, ServiceRatingUpdateDTO>
{
    /// <summary>
    /// الحصول على تقييمات خدمة معينة
    /// </summary>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <returns>قائمة بالتقييمات</returns>
    Task<IEnumerable<ServiceRatingReadDTO>> GetRatingsByServiceAsync(Guid serviceId);

    /// <summary>
    /// إضافة تقييم جديد للخدمة
    /// </summary>
    /// <param name="createDto">بيانات التقييم</param>
    /// <returns>DTO القراءة للتقييم المُنشأ</returns>
    Task<ServiceRatingReadDTO> AddRatingAsync(ServiceRatingCreateDTO createDto);

    /// <summary>
    /// حساب متوسط التقييم للخدمة
    /// </summary>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <returns>متوسط التقييم</returns>
    Task<decimal> CalculateAverageRatingAsync(Guid serviceId);
}
