using AfrahAPI.Models.DTOs.Booking;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة الحجوزات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    /// <summary>
    /// Constructor
    /// </summary>
    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// الحصول على جميع الحجوزات
    /// </summary>
    /// <returns>قائمة بجميع الحجوزات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingReadDTO>>> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }

    /// <summary>
    /// الحصول على حجز بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <returns>بيانات الحجز</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingReadDTO>> GetById(Guid id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        if (booking == null)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return Ok(booking);
    }

    /// <summary>
    /// إنشاء حجز جديد
    /// </summary>
    /// <param name="createDto">بيانات الحجز الجديد</param>
    /// <returns>بيانات الحجز المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<BookingReadDTO>> Create([FromBody] BookingCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = await _bookingService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
    }

    /// <summary>
    /// تحديث بيانات حجز موجود
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الحجز المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<BookingReadDTO>> Update(Guid id, [FromBody] BookingUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var booking = await _bookingService.UpdateAsync(id, updateDto);
        if (booking == null)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return Ok(booking);
    }

    /// <summary>
    /// حذف حجز
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _bookingService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// إنشاء حجز جديد مع التحقق من الصحة
    /// </summary>
    /// <param name="createDto">بيانات الحجز</param>
    /// <returns>بيانات الحجز المُنشأ</returns>
    [HttpPost("create-with-validation")]
    public async Task<ActionResult<BookingReadDTO>> CreateWithValidation([FromBody] BookingCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var booking = await _bookingService.CreateBookingWithValidationAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, booking);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// الحصول على حجوزات عميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بحجوزات العميل</returns>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<BookingReadDTO>>> GetByCustomer(Guid customerId)
    {
        var bookings = await _bookingService.GetBookingsByCustomerAsync(customerId);
        return Ok(bookings);
    }

    /// <summary>
    /// الحصول على حجوزات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بحجوزات الصالة</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<BookingReadDTO>>> GetByHall(Guid hallId)
    {
        var bookings = await _bookingService.GetBookingsByHallAsync(hallId);
        return Ok(bookings);
    }

    /// <summary>
    /// تحديث حالة الحجز
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <param name="status">الحالة الجديدة</param>
    /// <returns>بيانات الحجز المُحدث</returns>
    [HttpPut("{id}/status")]
    public async Task<ActionResult<BookingReadDTO>> UpdateStatus(Guid id, [FromBody] string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return BadRequest("حالة الحجز مطلوبة");

        var booking = await _bookingService.UpdateBookingStatusAsync(id, status);
        if (booking == null)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return Ok(booking);
    }

    /// <summary>
    /// إلغاء الحجز
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <param name="cancellationReason">سبب الإلغاء</param>
    /// <returns>بيانات الحجز المُحدث</returns>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<BookingReadDTO>> Cancel(Guid id, [FromBody] string cancellationReason)
    {
        if (string.IsNullOrWhiteSpace(cancellationReason))
            return BadRequest("سبب الإلغاء مطلوب");

        var booking = await _bookingService.CancelBookingAsync(id, cancellationReason);
        if (booking == null)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return Ok(booking);
    }

    /// <summary>
    /// تأكيد الحجز
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <returns>بيانات الحجز المُحدث</returns>
    [HttpPost("{id}/confirm")]
    public async Task<ActionResult<BookingReadDTO>> Confirm(Guid id)
    {
        var booking = await _bookingService.ConfirmBookingAsync(id);
        if (booking == null)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return Ok(booking);
    }

    /// <summary>
    /// وضع علامة على الحجز كمكتمل
    /// </summary>
    /// <param name="id">معرف الحجز</param>
    /// <returns>بيانات الحجز المُحدث</returns>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult<BookingReadDTO>> Complete(Guid id)
    {
        var booking = await _bookingService.CompleteBookingAsync(id);
        if (booking == null)
            return NotFound($"لم يتم العثور على حجز بالمعرف: {id}");

        return Ok(booking);
    }

    /// <summary>
    /// حساب تكلفة الحجز
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="startDate">تاريخ البدء</param>
    /// <param name="endDate">تاريخ الانتهاء</param>
    /// <returns>التكلفة الإجمالية</returns>
    [HttpGet("calculate-cost")]
    public async Task<ActionResult<decimal>> CalculateCost([FromQuery] Guid hallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate >= endDate)
            return BadRequest("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء");

        var cost = await _bookingService.CalculateBookingCostAsync(hallId, startDate, endDate);
        return Ok(new { hallId, startDate, endDate, totalCost = cost });
    }
}
