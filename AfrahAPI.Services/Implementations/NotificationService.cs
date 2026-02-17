using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Notification;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة الإشعارات
/// </summary>
public class NotificationService : BaseService<Notification, NotificationCreateDTO, NotificationReadDTO, NotificationUpdateDTO>, INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IPaymentRepository _paymentRepository;

    public NotificationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        INotificationRepository notificationRepository,
        IBookingRepository bookingRepository,
        IPaymentRepository paymentRepository)
        : base(unitOfWork, mapper, notificationRepository)
    {
        _notificationRepository = notificationRepository;
        _bookingRepository = bookingRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<NotificationReadDTO>> GetUserNotificationsAsync(Guid userId)
    {
        var notifications = await _notificationRepository.FindAsync(n => n.UserID == userId);
        return _mapper.Map<IEnumerable<NotificationReadDTO>>(notifications.OrderByDescending(n => n.CreatedAt));
    }

    public async Task<NotificationReadDTO?> MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null)
            return null;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;

        _notificationRepository.Update(notification);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<NotificationReadDTO>(notification);
    }

    public async Task<NotificationReadDTO> SendBookingConfirmationAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            throw new Exception("الحجز غير موجود");

        var notification = new Notification
        {
            NotificationID = Guid.NewGuid(),
            UserID = booking.Customer!.UserID,
            Title = "تأكيد الحجز",
            Message = $"تم تأكيد حجزك للصالة بنجاح. رقم الحجز: {booking.BookingId}",
            Type = "BookingConfirmation",
            RelatedEntityID = bookingId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<NotificationReadDTO>(notification);
    }

    public async Task<NotificationReadDTO> SendPaymentConfirmationAsync(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            throw new Exception("الدفع غير موجود");

        var notification = new Notification
        {
            NotificationID = Guid.NewGuid(),
            UserID = payment.Invoice!.Booking!.Customer!.UserID,
            Title = "تأكيد الدفع",
            Message = $"تم استلام دفعتك بمبلغ {payment.Amount} بنجاح",
            Type = "PaymentConfirmation",
            RelatedEntityID = paymentId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<NotificationReadDTO>(notification);
    }

    public async Task<NotificationReadDTO> SendBookingReminderAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            throw new Exception("الحجز غير موجود");

        var notification = new Notification
        {
            NotificationID = Guid.NewGuid(),
            UserID = booking.Customer!.UserID,
            Title = "تذكير بالحجز",
            Message = $"تذكير: لديك حجز قادم بتاريخ {booking.StartDate:dd/MM/yyyy}",
            Type = "BookingReminder",
            RelatedEntityID = bookingId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<NotificationReadDTO>(notification);
    }
}
