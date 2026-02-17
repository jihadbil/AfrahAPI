using AfrahAPI.Models.DTOs.Hall;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة الصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallController : ControllerBase
{
    private readonly IHallService _hallService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallController(IHallService hallService)
    {
        _hallService = hallService;
    }

    /// <summary>
    /// الحصول على جميع الصالات
    /// </summary>
    /// <returns>قائمة بجميع الصالات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallReadDTO>>> GetAll()
    {
        var halls = await _hallService.GetAllAsync();
        return Ok(halls);
    }

    /// <summary>
    /// الحصول على صالة بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف الصالة</param>
    /// <returns>بيانات الصالة</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallReadDTO>> GetById(Guid id)
    {
        var hall = await _hallService.GetByIdAsync(id);
        if (hall == null)
            return NotFound($"لم يتم العثور على صالة بالمعرف: {id}");

        return Ok(hall);
    }

    /// <summary>
    /// إنشاء صالة جديدة
    /// </summary>
    /// <param name="createDto">بيانات الصالة الجديدة</param>
    /// <returns>بيانات الصالة المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<HallReadDTO>> Create([FromBody] HallCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var hall = await _hallService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = hall.HallID }, hall);
    }

    /// <summary>
    /// تحديث بيانات صالة موجودة
    /// </summary>
    /// <param name="id">معرف الصالة</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الصالة المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallReadDTO>> Update(Guid id, [FromBody] HallUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var hall = await _hallService.UpdateAsync(id, updateDto);
        if (hall == null)
            return NotFound($"لم يتم العثور على صالة بالمعرف: {id}");

        return Ok(hall);
    }

    /// <summary>
    /// حذف صالة
    /// </summary>
    /// <param name="id">معرف الصالة</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على صالة بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// البحث عن الصالات مع تطبيق الفلاتر
    /// </summary>
    /// <param name="searchDto">معايير البحث</param>
    /// <returns>قائمة بالصالات المطابقة</returns>
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<HallReadDTO>>> Search([FromBody] HallSearchDTO searchDto)
    {
        var halls = await _hallService.SearchHallsAsync(searchDto);
        return Ok(halls);
    }

    /// <summary>
    /// الحصول على الصالات المتاحة لنطاق تواريخ معين
    /// </summary>
    /// <param name="startDate">تاريخ البدء</param>
    /// <param name="endDate">تاريخ الانتهاء</param>
    /// <returns>قائمة بالصالات المتاحة</returns>
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<HallReadDTO>>> GetAvailable([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate >= endDate)
            return BadRequest("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء");

        var halls = await _hallService.GetAvailableHallsAsync(startDate, endDate);
        return Ok(halls);
    }

    /// <summary>
    /// فحص توفر صالة معينة في نطاق تواريخ محدد
    /// </summary>
    /// <param name="id">معرف الصالة</param>
    /// <param name="startDate">تاريخ البدء</param>
    /// <param name="endDate">تاريخ الانتهاء</param>
    /// <returns>true إذا كانت الصالة متاحة، false خلاف ذلك</returns>
    [HttpGet("{id}/availability")]
    public async Task<ActionResult<bool>> CheckAvailability(Guid id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate >= endDate)
            return BadRequest("تاريخ البدء يجب أن يكون قبل تاريخ الانتهاء");

        var isAvailable = await _hallService.CheckHallAvailabilityAsync(id, startDate, endDate);
        return Ok(new { hallId = id, isAvailable, startDate, endDate });
    }

    /// <summary>
    /// الحصول على جميع صالات مالك معين
    /// </summary>
    /// <param name="ownerId">معرف صاحب الصالة</param>
    /// <returns>قائمة بصالات المالك</returns>
    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<IEnumerable<HallReadDTO>>> GetByOwner(Guid ownerId)
    {
        var halls = await _hallService.GetHallsByOwnerAsync(ownerId);
        return Ok(halls);
    }

    /// <summary>
    /// الحصول على الصالات حسب الفئة
    /// </summary>
    /// <param name="categoryId">معرف الفئة</param>
    /// <returns>قائمة بالصالات في هذه الفئة</returns>
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<HallReadDTO>>> GetByCategory(Guid categoryId)
    {
        var halls = await _hallService.GetHallsByCategoryAsync(categoryId);
        return Ok(halls);
    }

    /// <summary>
    /// التحقق من الصالة (للمسؤول فقط)
    /// </summary>
    /// <param name="id">معرف الصالة</param>
    /// <returns>بيانات الصالة المُحدثة</returns>
    [HttpPost("{id}/verify")]
    public async Task<ActionResult<HallReadDTO>> Verify(Guid id)
    {
        var hall = await _hallService.VerifyHallAsync(id);
        if (hall == null)
            return NotFound($"لم يتم العثور على صالة بالمعرف: {id}");

        return Ok(hall);
    }

    /// <summary>
    /// تحديث حالة توفر الصالة
    /// </summary>
    /// <param name="id">معرف الصالة</param>
    /// <param name="isAvailable">الحالة الجديدة</param>
    /// <returns>بيانات الصالة المُحدثة</returns>
    [HttpPut("{id}/availability")]
    public async Task<ActionResult<HallReadDTO>> UpdateAvailability(Guid id, [FromBody] bool isAvailable)
    {
        var hall = await _hallService.UpdateHallAvailabilityAsync(id, isAvailable);
        if (hall == null)
            return NotFound($"لم يتم العثور على صالة بالمعرف: {id}");

        return Ok(hall);
    }
}
