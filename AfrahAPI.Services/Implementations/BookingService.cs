using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Booking;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة الحجوزات
/// </summary>
public class BookingService : BaseService<Booking, BookingCreateDTO, BookingReadDTO, BookingUpdateDTO>, IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHallRepository _hallRepository;
    private readonly IHallUnavailableDateRepository _unavailableDateRepository;
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// المُنشئ
    /// </summary>
    public BookingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBookingRepository bookingRepository,
        IHallRepository hallRepository,
        IHallUnavailableDateRepository unavailableDateRepository,
        ICustomerRepository customerRepository)
        : base(unitOfWork, mapper, bookingRepository)
    {
        _bookingRepository = bookingRepository;
        _hallRepository = hallRepository;
        _unavailableDateRepository = unavailableDateRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// إنشاء حجز جديد مع التحقق من الصحة
    /// </summary>
    public async Task<BookingReadDTO> CreateBookingWithValidationAsync(BookingCreateDTO createDto)
    {
        // التحقق من وجود الصالة
        var hall = await _hallRepository.GetByIdAsync(createDto.HallId);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        if (!hall.IsAvailable)
            throw new Exception("الصالة غير متاحة للحجز حالياً");

        // التحقق من وجود العميل
        var customer = await _customerRepository.GetByIdAsync(createDto.CustomerId);
        if (customer == null)
            throw new Exception("العميل غير موجود");

        // التحقق من التواريخ
        if (createDto.StartDate >= createDto.EndDate)
            throw new Exception("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء");

        if (createDto.StartDate < DateTime.UtcNow)
            throw new Exception("لا يمكن الحجز في تواريخ سابقة");

        // التحقق من توفر الصالة
        var isAvailable = await CheckAvailability(createDto.HallId, createDto.StartDate, createDto.EndDate);
        if (!isAvailable)
            throw new Exception("الصالة محجوزة في هذه التواريخ");

        // حساب التكلفة
        var totalCost  = await CalculateBookingCostAsync(createDto.HallId, createDto.StartDate, createDto.EndDate);

        // إنشاء الحجز
        var booking = _mapper.Map<Booking>(createDto);
        booking.BookingId = Guid.NewGuid();
        booking.CreatedAt = DateTime.UtcNow;
        booking.TotalPrice = totalCost;
        booking.Status = hall.AutoAcceptReservations ? "Confirmed" : "Pending";
        booking.DepositAmount = hall.DefaultDepositAmount;

        await _bookingRepository.AddAsync(booking);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<BookingReadDTO>(booking);
    }

    /// <summary>
    /// الحصول على حجوزات عميل معين
    /// </summary>
    public async Task<IEnumerable<BookingReadDTO>> GetBookingsByCustomerAsync(Guid customerId)
    {
        var bookings = await _bookingRepository.FindAsync(b => b.CustomerId == customerId);
        return _mapper.Map<IEnumerable<BookingReadDTO>>(bookings);
    }

    /// <summary>
    /// الحصول على حجوزات صالة معينة
    /// </summary>
    public async Task<IEnumerable<BookingReadDTO>> GetBookingsByHallAsync(Guid hallId)
    {
        var bookings = await _bookingRepository.FindAsync(b => b.HallId == hallId);
        return _mapper.Map<IEnumerable<BookingReadDTO>>(bookings);
    }

    /// <summary>
    /// تحديث حالة الحجز
    /// </summary>
    public async Task<BookingReadDTO?> UpdateBookingStatusAsync(Guid bookingId, string status)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            return null;

        // التحقق من صحة الحالة
        var validStatuses = new[] { "Pending", "Confirmed", "Cancelled", "Completed" };
        if (!validStatuses.Contains(status))
            throw new Exception("حالة غير صحيحة");

        booking.Status = status;

        if (status == "Cancelled")
        {
            booking.UpdatedAt = DateTime.UtcNow;
        }

        _bookingRepository.Update(booking);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<BookingReadDTO>(booking);
    }

    /// <summary>
    /// إلغاء الحجز
    /// </summary>
    public async Task<BookingReadDTO?> CancelBookingAsync(Guid bookingId, string cancellationReason)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            return null;

        if (booking.Status == "Cancelled" || booking.Status == "Completed")
            throw new Exception("لا يمكن إلغاء هذا الحجز");

        booking.Status = "Cancelled";
        booking.Notes = cancellationReason;
        booking.UpdatedAt = DateTime.UtcNow;

        _bookingRepository.Update(booking);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<BookingReadDTO>(booking);
    }

    /// <summary>
    /// تأكيد الحجز
    /// </summary>
    public async Task<BookingReadDTO?> ConfirmBookingAsync(Guid bookingId)
    {
        return await UpdateBookingStatusAsync(bookingId, "Confirmed");
    }

    /// <summary>
    /// وضع علامة على الحجز كمكتمل
    /// </summary>
    public async Task<BookingReadDTO?> CompleteBookingAsync(Guid bookingId)
    {
        return await UpdateBookingStatusAsync(bookingId, "Completed");
    }

    /// <summary>
    /// حساب تكلفة الحجز
    /// </summary>
    public async Task<decimal> CalculateBookingCostAsync(Guid hallId, DateTime startDate, DateTime endDate)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        var duration = endDate - startDate;

        // حساب التكلفة بناءً على طريقة التسعير
        decimal cost = 0;

        if (hall.PricingMode == "PerDay")
        {
            var days = (int)Math.Ceiling(duration.TotalDays);
            cost = days * hall.PricePerDay;
        }
        else if (hall.PricingMode == "PerHour")
        {
            var hours = (int)Math.Ceiling(duration.TotalHours);
            cost = hours * hall.PricePerHour;
        }
        else
        {
            // إذا كان مخصص، استخدم السعر اليومي كقيمة افتراضية
            var days = (int)Math.Ceiling(duration.TotalDays);
            cost = days * hall.PricePerDay;
        }

        return cost;
    }

    /// <summary>
    /// التحقق من توفر الصالة (دالة مساعدة خاصة)
    /// </summary>
    private async Task<bool> CheckAvailability(Guid hallId, DateTime startDate, DateTime endDate)
    {
        // فحص التواريخ المحجوبة
        var unavailableDates = await _unavailableDateRepository.FindAsync(
            ud => ud.HallID == hallId &&
                  ((ud.StartDateTime <= endDate && ud.EndDateTime >= startDate)));

        if (unavailableDates.Any())
            return false;

        // فحص الحجوزات الموجودة
        var existingBookings = await _bookingRepository.FindAsync(
            b => b.HallId == hallId &&
                 b.Status != "Cancelled" &&
                 ((b.StartDate <= endDate && b.EndDate >= startDate)));

        if (existingBookings.Any())
            return false;

        return true;
    }
}
