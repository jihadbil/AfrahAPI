using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallRating;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة تقييمات الصالات
/// </summary>
public interface IHallRatingService : IBaseService<HallRating, HallRatingCreateDTO, HallRatingReadDTO, HallRatingUpdateDTO>
{
    /// <summary>
    /// الحصول على تقييمات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتقييمات</returns>
    Task<IEnumerable<HallRatingReadDTO>> GetRatingsByHallAsync(Guid hallId);

    /// <summary>
    /// إضافة تقييم جديد للصالة
    /// </summary>
    /// <param name="createDto">بيانات التقييم</param>
    /// <returns>DTO القراءة للتقييم المُنشأ</returns>
    Task<HallRatingReadDTO> AddRatingAsync(HallRatingCreateDTO createDto);

    /// <summary>
    /// حساب متوسط التقييم للصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>متوسط التقييم</returns>
    Task<decimal> CalculateAverageRatingAsync(Guid hallId);
}
