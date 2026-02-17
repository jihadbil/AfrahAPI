using AfrahAPI.Models.DTOs.PaymentMethod;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة طرق الدفع
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodService _paymentMethodService;

    /// <summary>
    /// Constructor
    /// </summary>
    public PaymentMethodController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    /// <summary>
    /// الحصول على جميع طرق الدفع
    /// </summary>
    /// <returns>قائمة بجمي طرق الدفع</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentMethodReadDTO>>> GetAll()
    {
        var methods = await _paymentMethodService.GetAllAsync();
        return Ok(methods);
    }

    /// <summary>
    /// الحصول على طريقة دفع بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف طريقة الدفع</param>
    /// <returns>بيانات طريقة الدفع</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentMethodReadDTO>> GetById(Guid id)
    {
        var method = await _paymentMethodService.GetByIdAsync(id);
        if (method == null)
            return NotFound($"لم يتم العثور على طريقة دفع بالمعرف: {id}");

        return Ok(method);
    }

    /// <summary>
    /// إنشاء طريقة دفع جديدة
    /// </summary>
    /// <param name="createDto">بيانات طريقة الدفع الجديدة</param>
    /// <returns>بيانات طريقة الدفع المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<PaymentMethodReadDTO>> Create([FromBody] PaymentMethodCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var method = await _paymentMethodService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = method.MethodId }, method);
    }

    /// <summary>
    /// تحديث بيانات طريقة دفع موجودة
    /// </summary>
    /// <param name="id">معرف طريقة الدفع</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات طريقة الدفع المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PaymentMethodReadDTO>> Update(Guid id, [FromBody] PaymentMethodUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var method = await _paymentMethodService.UpdateAsync(id, updateDto);
        if (method == null)
            return NotFound($"لم يتم العثور على طريقة دفع بالمعرف: {id}");

        return Ok(method);
    }

    /// <summary>
    /// حذف طريقة دفع
    /// </summary>
    /// <param name="id">معرف طريقة الدفع</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _paymentMethodService.DeleteAsync(id);
        if(!result)
            return NotFound($"لم يتم العثور على طريقة دفع بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على جميع طرق الدفع النشطة
    /// </summary>
    /// <returns>قائمة بطرق الدفع النشطة</returns>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<PaymentMethodReadDTO>>> GetActive()
    {
        var methods = await _paymentMethodService.GetActivePaymentMethodsAsync();
        return Ok(methods);
    }
}
