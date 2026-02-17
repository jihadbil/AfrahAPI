using AfrahAPI.Models.DTOs.HallPaymentMethod;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة طرق الدفع للصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallPaymentMethodController : ControllerBase
{
    private readonly IHallPaymentMethodService _hallPaymentMethodService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallPaymentMethodController(IHallPaymentMethodService hallPaymentMethodService)
    {
        _hallPaymentMethodService = hallPaymentMethodService;
    }

    /// <summary>
    /// الحصول على جميع ربط طرق الدفع
    /// </summary>
    /// <returns>قائمة بجميع الربط</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallPaymentMethodReadDTO>>> GetAll()
    {
        var methods = await _hallPaymentMethodService.GetAllAsync();
        return Ok(methods);
    }

    /// <summary>
    /// الحصول على ربط بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف الربط</param>
    /// <returns>بيانات الربط</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallPaymentMethodReadDTO>> GetById(Guid id)
    {
        var method = await _hallPaymentMethodService.GetByIdAsync(id);
        if (method == null)
            return NotFound($"لم يتم العثور على ربط بالمعرف: {id}");

        return Ok(method);
    }

    /// <summary>
    /// إنشاء ربط جديد
    /// </summary>
    /// <param name="createDto">بيانات الربط الجديد</param>
    /// <returns>بيانات الربط المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<HallPaymentMethodReadDTO>> Create([FromBody] HallPaymentMethodCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var method = await _hallPaymentMethodService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = method.HallPaymentMethodID }, method);
    }

    /// <summary>
    /// تحديث بيانات ربط موجود
    /// </summary>
    /// <param name="id">معرف الربط</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الربط المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallPaymentMethodReadDTO>> Update(Guid id, [FromBody] HallPaymentMethodUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var method = await _hallPaymentMethodService.UpdateAsync(id, updateDto);
        if (method == null)
            return NotFound($"لم يتم العثور على ربط بالمعرف: {id}");

        return Ok(method);
    }

    /// <summary>
    /// حذف ربط
    /// </summary>
    /// <param name="id">معرف الربط</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallPaymentMethodService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على ربط بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على طرق الدفع المتاحة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بطرق الدفع</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<HallPaymentMethodReadDTO>>> GetByHall(Guid hallId)
    {
        var methods = await _hallPaymentMethodService.GetHallPaymentMethodsAsync(hallId);
        return Ok(methods);
    }

    /// <summary>
    /// الحصول على الصالات التي تدعم طريقة دفع معينة
    /// </summary>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>قائمة بالصالات</returns>
    [HttpGet("payment-method/{paymentMethodId}")]
    public async Task<ActionResult<IEnumerable<HallPaymentMethodReadDTO>>> GetByPaymentMethod(Guid paymentMethodId)
    {
        var halls = await _hallPaymentMethodService.GetHallsByPaymentMethodAsync(paymentMethodId);
        return Ok(halls);
    }

    /// <summary>
    /// إضافة طريقة دفع لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>بيانات الربط المُنشأ</returns>
    [HttpPost("hall/{hallId}/add/{paymentMethodId}")]
    public async Task<ActionResult<HallPaymentMethodReadDTO>> AddToHall(Guid hallId, Guid paymentMethodId)
    {
        try
        {
            var method = await _hallPaymentMethodService.AddPaymentMethodToHallAsync(hallId, paymentMethodId);
            return CreatedAtAction(nameof(GetById), new { id = method.HallPaymentMethodID }, method);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// إزالة طريقة دفع من صالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="paymentMethodId">معرف طريقة الدفع</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("hall/{hallId}/remove/{paymentMethodId}")]
    public async Task<ActionResult> RemoveFromHall(Guid hallId, Guid paymentMethodId)
    {
        var result = await _hallPaymentMethodService.RemovePaymentMethodFromHallAsync(hallId, paymentMethodId);
        if (!result)
            return NotFound($"لم يتم العثور على ربط للصالة والطريقة المحددة");

        return NoContent();
    }
}
