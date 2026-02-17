using AfrahAPI.Models.DTOs.Payment;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة المدفوعات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    /// Constructor
    /// </summary>
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// الحصول على جميع المدفوعات
    /// </summary>
    /// <returns>قائمة بجميع المدفوعات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetAll()
    {
        var payments = await _paymentService.GetAllAsync();
        return Ok(payments);
    }

    /// <summary>
    /// الحصول على مدفوعة بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف المدفوعة</param>
    /// <returns>بيانات المدفوعة</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentReadDTO>> GetById(Guid id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null)
            return NotFound($"لم يتم العثور على مدفوعة بالمعرف: {id}");

        return Ok(payment);
    }

    /// <summary>
    /// إنشاء مدفوعة جديدة
    /// </summary>
    /// <param name="createDto">بيانات المدفوعة الجديدة</param>
    /// <returns>بيانات المدفوعة المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<PaymentReadDTO>> Create([FromBody] PaymentCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var payment = await _paymentService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
    }

    /// <summary>
    /// تحديث بيانات مدفوعة موجودة
    /// </summary>
    /// <param name="id">معرف المدفوعة</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات المدفوعة المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PaymentReadDTO>> Update(Guid id, [FromBody] PaymentUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var payment = await _paymentService.UpdateAsync(id, updateDto);
        if (payment == null)
            return NotFound($"لم يتم العثور على مدفوعة بالمعرف: {id}");

        return Ok(payment);
    }

    /// <summary>
    /// حذف مدفوعة
    /// </summary>
    /// <param name="id">معرف المدفوعة</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _paymentService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على مدفوعة بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// معالجة دفعة جديدة
    /// </summary>
    /// <param name="createDto">بيانات الدفع</param>
    /// <returns>بيانات الدفع المُنشأ</returns>
    [HttpPost("process")]
    public async Task<ActionResult<PaymentReadDTO>> Process([FromBody] PaymentCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var payment = await _paymentService.ProcessPaymentAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// الحصول على مدفوعات فاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>قائمة بالمدفوعات</returns>
    [HttpGet("invoice/{invoiceId}")]
    public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetByInvoice(Guid invoiceId)
    {
        var payments = await _paymentService.GetPaymentsByInvoiceAsync(invoiceId);
        return Ok(payments);
    }

    /// <summary>
    /// معالجة استرداد مبلغ
    /// </summary>
    /// <param name="id">معرف الدفع</param>
    /// <param name="amount">المبلغ المسترد</param>
    /// <returns>بيانات الدفع المُحدث</returns>
    [HttpPost("{id}/refund")]
    public async Task<ActionResult<PaymentReadDTO>> Refund(Guid id, [FromBody] decimal amount)
    {
        if (amount <= 0)
            return BadRequest("المبلغ المسترد يجب أن يكون أكبر من صفر");

        try
        {
            var payment = await _paymentService.RefundPaymentAsync(id, amount);
            if (payment == null)
                return NotFound($"لم يتم العثور على مدفوعة بالمعرف: {id}");

            return Ok(payment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
