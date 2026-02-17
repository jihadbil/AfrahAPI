using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Booking;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة الحجوزات
/// </summary>
public interface IBookingService : IBaseService<Booking, BookingCreateDTO, BookingReadDTO, BookingUpdateDTO>
{
    /// <summary>
    /// إنشاء حجز جديد مع التحقق من الصحة
    /// </summary>
    /// <param name="createDto">بيانات الحجز</param>
    /// <returns>DTO القراءة للحجز المُنشأ</returns>
    Task<BookingReadDTO> CreateBookingWithValidationAsync(BookingCreateDTO createDto);

    /// <summary>
    /// الحصول على حجوزات عميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بحجوزات العميل</returns>
    Task<IEnumerable<BookingReadDTO>> GetBookingsByCustomerAsync(Guid customerId);

    /// <summary>
    /// الحصول على حجوزات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بحجوزات الصالة</returns>
    Task<IEnumerable<BookingReadDTO>> GetBookingsByHallAsync(Guid hallId);

    /// <summary>
    /// تحديث حالة الحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <param name="status">الحالة الجديدة</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<BookingReadDTO?> UpdateBookingStatusAsync(Guid bookingId, string status);

    /// <summary>
    /// إلغاء الحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <param name="cancellationReason">سبب الإلغاء</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<BookingReadDTO?> CancelBookingAsync(Guid bookingId, string cancellationReason);

    /// <summary>
    /// تأكيد الحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<BookingReadDTO?> ConfirmBookingAsync(Guid bookingId);

    /// <summary>
    /// وضع علامة على الحجز كمكتمل
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<BookingReadDTO?> CompleteBookingAsync(Guid bookingId);

    /// <summary>
    /// حساب تكلفة الحجز
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="startDate">تاريخ البدء</param>
    /// <param name="endDate">تاريخ الانتهاء</param>
    /// <returns>التكلفة الإجمالية</returns>
    Task<decimal> CalculateBookingCostAsync(Guid hallId, DateTime startDate, DateTime endDate);
}
