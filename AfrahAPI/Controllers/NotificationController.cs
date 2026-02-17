using AfrahAPI.Models.DTOs.Notification;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة الإشعارات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    /// <summary>
    /// Constructor
    /// </summary>
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// الحصول على جميع الإشعارات
    /// </summary>
    /// <returns>قائمة بجميع الإشعارات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationReadDTO>>> GetAll()
    {
        var notifications = await _notificationService.GetAllAsync();
        return Ok(notifications);
    }

    /// <summary>
    /// الحصول على إشعار بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف الإشعار</param>
    /// <returns>بيانات الإشعار</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<NotificationReadDTO>> GetById(Guid id)
    {
        var notification = await _notificationService.GetByIdAsync(id);
        if (notification == null)
            return NotFound($"لم يتم العثور على إشعار بالمعرف: {id}");

        return Ok(notification);
    }

    /// <summary>
    /// إنشاء إشعار جديد
    /// </summary>
    /// <param name="createDto">بيانات الإشعار الجديد</param>
    /// <returns>بيانات الإشعار المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<NotificationReadDTO>> Create([FromBody] NotificationCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var notification = await _notificationService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = notification.NotificationID }, notification);
    }

    /// <summary>
    /// تحديث بيانات إشعار موجود
    /// </summary>
    /// <param name="id">معرف الإشعار</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الإشعار المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<NotificationReadDTO>> Update(Guid id, [FromBody] NotificationUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var notification = await _notificationService.UpdateAsync(id, updateDto);
        if (notification == null)
            return NotFound($"لم يتم العثور على إشعار بالمعرف: {id}");

        return Ok(notification);
    }

    /// <summary>
    /// حذف إشعار
    /// </summary>
    /// <param name="id">معرف الإشعار</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _notificationService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على إشعار بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على إشعارات مستخدم معين
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>قائمة بالإشعارات</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<NotificationReadDTO>>> GetByUser(Guid userId)
    {
        var notifications = await _notificationService.GetUserNotificationsAsync(userId);
        return Ok(notifications);
    }

    /// <summary>
    /// وضع علامة مقروء على إشعار
    /// </summary>
    /// <param name="id">معرف الإشعار</param>
    /// <returns>بيانات الإشعار المُحدث</returns>
    [HttpPost("{id}/mark-read")]
    public async Task<ActionResult<NotificationReadDTO>> MarkAsRead(Guid id)
    {
        var notification = await _notificationService.MarkAsReadAsync(id);
        if (notification == null)
            return NotFound($"لم يتم العثور على إشعار بالمعرف: {id}");

        return Ok(notification);
    }

    /// <summary>
    /// إرسال تأكيد الحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>الإشعار المُنشأ</returns>
    [HttpPost("booking-confirmation/{bookingId}")]
    public async Task<ActionResult<NotificationReadDTO>> SendBookingConfirmation(Guid bookingId)
    {
        try
        {
            var notification = await _notificationService.SendBookingConfirmationAsync(bookingId);
            return CreatedAtAction(nameof(GetById), new { id = notification.NotificationID }, notification);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// إرسال تأكيد الدفع
    /// </summary>
    /// <param name="paymentId">معرف الدفع</param>
    /// <returns>الإشعار المُنشأ</returns>
    [HttpPost("payment-confirmation/{paymentId}")]
    public async Task<ActionResult<NotificationReadDTO>> SendPaymentConfirmation(Guid paymentId)
    {
        try
        {
            var notification = await _notificationService.SendPaymentConfirmationAsync(paymentId);
            return CreatedAtAction(nameof(GetById), new { id = notification.NotificationID }, notification);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// إرسال تذكير بالحجز
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>الإشعار المُنشأ</returns>
    [HttpPost("booking-reminder/{bookingId}")]
    public async Task<ActionResult<NotificationReadDTO>> SendBookingReminder(Guid bookingId)
    {
        try
        {
            var notification = await _notificationService.SendBookingReminderAsync(bookingId);
            return CreatedAtAction(nameof(GetById), new { id = notification.NotificationID }, notification);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
