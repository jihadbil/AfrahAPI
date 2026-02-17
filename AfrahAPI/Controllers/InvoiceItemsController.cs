using AfrahAPI.Models.DTOs.InvoiceItems;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة بنود الفاتورة
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceItemsController : ControllerBase
{
    private readonly IInvoiceItemsService _invoiceItemsService;

    /// <summary>
    /// Constructor
    /// </summary>
    public InvoiceItemsController(IInvoiceItemsService invoiceItemsService)
    {
        _invoiceItemsService = invoiceItemsService;
    }

    /// <summary>
    /// الحصول على جميع البنود
    /// </summary>
    /// <returns>قائمة بجميع البنود</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceItemsReadDTO>>> GetAll()
    {
        var items = await _invoiceItemsService.GetAllAsync();
        return Ok(items);
    }

    /// <summary>
    /// الحصول على بند بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف البند</param>
    /// <returns>بيانات البند</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceItemsReadDTO>> GetById(Guid id)
    {
        var item = await _invoiceItemsService.GetByIdAsync(id);
        if (item == null)
            return NotFound($"لم يتم العثور على بند بالمعرف: {id}");

        return Ok(item);
    }

    /// <summary>
    /// إنشاء بند جديد
    /// </summary>
    /// <param name="createDto">بيانات البند الجديد</param>
    /// <returns>بيانات البند المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<InvoiceItemsReadDTO>> Create([FromBody] InvoiceItemsCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var item = await _invoiceItemsService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
    }

    /// <summary>
    /// تحديث بيانات بند موجود
    /// </summary>
    /// <param name="id">معرف البند</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات البند المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<InvoiceItemsReadDTO>> Update(Guid id, [FromBody] InvoiceItemsUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var item = await _invoiceItemsService.UpdateAsync(id, updateDto);
        if (item == null)
            return NotFound($"لم يتم العثور على بند بالمعرف: {id}");

        return Ok(item);
    }

    /// <summary>
    /// حذف بند
    /// </summary>
    /// <param name="id">معرف البند</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _invoiceItemsService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على بند بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على بنود فاتورة معينة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>قائمة بالبنود</returns>
    [HttpGet("invoice/{invoiceId}")]
    public async Task<ActionResult<IEnumerable<InvoiceItemsReadDTO>>> GetByInvoice(Guid invoiceId)
    {
        var items = await _invoiceItemsService.GetItemsByInvoiceAsync(invoiceId);
        return Ok(items);
    }

    /// <summary>
    /// إضافة بند لفاتورة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <param name="createDto">بيانات البند</param>
    /// <returns>بيانات البند المُنشأ</returns>
    [HttpPost("invoice/{invoiceId}/add")]
    public async Task<ActionResult<InvoiceItemsReadDTO>> AddToInvoice(Guid invoiceId, [FromBody] InvoiceItemsCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var item = await _invoiceItemsService.AddItemToInvoiceAsync(invoiceId, createDto);
            return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// حساب إجمالي البند
    /// </summary>
    /// <param name="id">معرف البند</param>
    /// <returns>الإجمالي</returns>
    [HttpGet("{id}/total")]
    public async Task<ActionResult<decimal>> GetItemTotal(Guid id)
    {
        try
        {
            var total = await _invoiceItemsService.CalculateItemTotalAsync(id);
            return Ok(new { itemId = id, total });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// حساب إجمالي جميع بنود الفاتورة
    /// </summary>
    /// <param name="invoiceId">معرف الفاتورة</param>
    /// <returns>الإجمالي الكلي</returns>
    [HttpGet("invoice/{invoiceId}/total")]
    public async Task<ActionResult<decimal>> GetInvoiceTotal(Guid invoiceId)
    {
        var total = await _invoiceItemsService.CalculateInvoiceItemsTotalAsync(invoiceId);
        return Ok(new { invoiceId, total });
    }
}
