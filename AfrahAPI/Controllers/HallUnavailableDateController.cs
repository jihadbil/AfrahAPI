using AfrahAPI.Models.DTOs.HallUnavailableDate;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة التواريخ غير المتاحة للصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallUnavailableDateController : ControllerBase
{
    private readonly IHallUnavailableDateService _hallUnavailableDateService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallUnavailableDateController(IHallUnavailableDateService hallUnavailableDateService)
    {
        _hallUnavailableDateService = hallUnavailableDateService;
    }

    /// <summary>
    /// الحصول على جميع التواريخ المحجوبة
    /// </summary>
    /// <returns>قائمة بجميع التواريخ المحجوبة</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallUnavailableDateReadDTO>>> GetAll()
    {
        var dates = await _hallUnavailableDateService.GetAllAsync();
        return Ok(dates);
    }

    /// <summary>
    /// الحصول على تاريخ محجوب بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف التاريخ المحجوب</param>
    /// <returns>بيانات التاريخ المحجوب</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallUnavailableDateReadDTO>> GetById(Guid id)
    {
        var date = await _hallUnavailableDateService.GetByIdAsync(id);
        if (date == null)
            return NotFound($"لم يتم العثور على تاريخ محجوب بالمعرف: {id}");

        return Ok(date);
    }

    /// <summary>
    /// إنشاء تاريخ محجوب جديد
    /// </summary>
    /// <param name="createDto">بيانات التاريخ المحجوب الجديد</param>
    /// <returns>بيانات التاريخ المحجوب المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<HallUnavailableDateReadDTO>> Create([FromBody] HallUnavailableDateCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var date = await _hallUnavailableDateService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = date.UnavailableID }, date);
    }

    /// <summary>
    /// تحديث بيانات تاريخ محجوب موجود
    /// </summary>
    /// <param name="id">معرف التاريخ المحجوب</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات التاريخ المحجوب المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallUnavailableDateReadDTO>> Update(Guid id, [FromBody] HallUnavailableDateUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var date = await _hallUnavailableDateService.UpdateAsync(id, updateDto);
        if (date == null)
            return NotFound($"لم يتم العثور على تاريخ محجوب بالمعرف: {id}");

        return Ok(date);
    }

    /// <summary>
    /// حذف تاريخ محجوب
    /// </summary>
    /// <param name="id">معرف التاريخ المحجوب</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallUnavailableDateService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على تاريخ محجوب بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على التواريخ المحجوبة لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتواريخ المحجوبة</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<HallUnavailableDateReadDTO>>> GetByHall(Guid hallId)
    {
        var dates = await _hallUnavailableDateService.GetUnavailableDatesByHallIdAsync(hallId);
        return Ok(dates);
    }

    /// <summary>
    /// إضافة تاريخ محجوب لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="createDto">بيانات التاريخ المحجوب</param>
    /// <returns>بيانات التاريخ المحجوب المُنشأ</returns>
    [HttpPost("hall/{hallId}/add")]
    public async Task<ActionResult<HallUnavailableDateReadDTO>> AddToHall(Guid hallId, [FromBody] HallUnavailableDateCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var date = await _hallUnavailableDateService.AddUnavailableDateAsync(hallId, createDto);
            return CreatedAtAction(nameof(GetById), new { id = date.UnavailableID }, date);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
