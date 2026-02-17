using AfrahAPI.Models.DTOs.Invoice;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة الفواتير
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    /// <summary>
    /// Constructor
    /// </summary>
    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    /// <summary>
    /// الحصول على جميع الفواتير
    /// </summary>
    /// <returns>قائمة بجميع الفواتير</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceReadDTO>>> GetAll()
    {
        var invoices = await _invoiceService.GetAllAsync();
        return Ok(invoices);
    }

    /// <summary>
    /// الحصول على فاتورة بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف الفاتورة</param>
    /// <returns>بيانات الفاتورة</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceReadDTO>> GetById(Guid id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice == null)
            return NotFound($"لم يتم العثور على فاتورة بالمعرف: {id}");

        return Ok(invoice);
    }

    /// <summary>
    /// إنشاء فاتورة جديدة
    /// </summary>
    /// <param name="createDto">بيانات الفاتورة الجديدة</param>
    /// <returns>بيانات الفاتورة المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<InvoiceReadDTO>> Create([FromBody] InvoiceCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var invoice = await _invoiceService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = invoice.InvoiceId }, invoice);
    }

    /// <summary>
    /// تحديث بيانات فاتورة موجودة
    /// </summary>
    /// <param name="id">معرف الفاتورة</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الفاتورة المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceReadDTO>> Update(Guid id, [FromBody] InvoiceUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var invoice = await _invoiceService.UpdateAsync(id, updateDto);
        if (invoice == null)
            return NotFound($"لم يتم العثور على فاتورة بالمعرف: {id}");

        return Ok(invoice);
    }

    /// <summary>
    /// حذف فاتورة
    /// </summary>
    /// <param name="id">معرف الفاتورة</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _invoiceService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على فاتورة بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// إنشاء فاتورة لحجز معين
    /// </summary>
    /// <param name="bookingId">معرف الحجز</param>
    /// <returns>بيانات الفاتورة المُنشأة</returns>
    [HttpPost("generate/{bookingId}")]
    public async Task<ActionResult<InvoiceReadDTO>> GenerateForBooking(Guid bookingId)
    {
        try
        {
            var invoice = await _invoiceService.GenerateInvoiceForBookingAsync(bookingId);
            return CreatedAtAction(nameof(GetById), new { id = invoice.InvoiceId }, invoice);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// الحصول على فواتير عميل معين
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بالفواتير</returns>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<InvoiceReadDTO>>> GetByCustomer(Guid customerId)
    {
        var invoices = await _invoiceService.GetInvoicesByCustomerAsync(customerId);
        return Ok(invoices);
    }

    /// <summary>
    /// حساب إجمالي الفاتورة مع البنود
    /// </summary>
    /// <param name="id">معرف الفاتورة</param>
    /// <returns>الإجمالي</returns>
    [HttpGet("{id}/total")]
    public async Task<ActionResult<decimal>> GetTotal(Guid id)
    {
        try
        {
            var total = await _invoiceService.CalculateInvoiceTotalAsync(id);
            return Ok(new { invoiceId = id, total });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
