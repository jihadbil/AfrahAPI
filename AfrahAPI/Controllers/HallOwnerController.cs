using AfrahAPI.Models.DTOs.HallOwner;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة أصحاب الصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallOwnerController : ControllerBase
{
    private readonly IHallOwnerService _hallOwnerService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallOwnerController(IHallOwnerService hallOwnerService)
    {
        _hallOwnerService = hallOwnerService;
    }

    /// <summary>
    /// الحصول على جميع أصحاب الصالات
    /// </summary>
    /// <returns>قائمة بجميع أصحاب الصالات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallOwnerReadDTO>>> GetAll()
    {
        var owners = await _hallOwnerService.GetAllAsync();
        return Ok(owners);
    }

    /// <summary>
    /// الحصول على صاحب صالة بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف صاحب الصالة</param>
    /// <returns>بيانات صاحب الصالة</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallOwnerReadDTO>> GetById(Guid id)
    {
        var owner = await _hallOwnerService.GetByIdAsync(id);
        if (owner == null)
            return NotFound($"لم يتم العثور على صاحب صالة بالمعرف: {id}");

        return Ok(owner);
    }

    /// <summary>
    /// إنشاء صاحب صالة جديد
    /// </summary>
    /// <param name="createDto">بيانات صاحب الصالة الجديد</param>
    /// <returns>بيانات صاحب الصالة المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<HallOwnerReadDTO>> Create([FromBody] HallOwnerCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var owner = await _hallOwnerService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = owner.OwnerID }, owner);
    }

    /// <summary>
    /// تحديث بيانات صاحب صالة موجود
    /// </summary>
    /// <param name="id">معرف صاحب الصالة</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات صاحب الصالة المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallOwnerReadDTO>> Update(Guid id, [FromBody] HallOwnerUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var owner = await _hallOwnerService.UpdateAsync(id, updateDto);
        if (owner == null)
            return NotFound($"لم يتم العثور على صاحب صالة بالمعرف: {id}");

        return Ok(owner);
    }

    /// <summary>
    /// حذف صاحب صالة
    /// </summary>
    /// <param name="id">معرف صاحب الصالة</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallOwnerService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على صاحب صالة بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على صاحب صالة بواسطة معرف المستخدم
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>بيانات صاحب الصالة</returns>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<HallOwnerReadDTO>> GetByUserId(Guid userId)
    {
        var owner = await _hallOwnerService.GetHallOwnerByUserIdAsync(userId);
        if (owner == null)
            return NotFound($"لم يتم العثور على صاحب صالة لمعرف المستخدم: {userId}");

        return Ok(owner);
    }

    /// <summary>
    /// الحصول على جميع الصالات المملوكة لهذا المالك
    /// </summary>
    /// <param name="id">معرف صاحب الصالة</param>
    /// <returns>قائمة بالصالات</returns>
    [HttpGet("{id}/halls")]
    public async Task<ActionResult<IEnumerable<object>>> GetHalls(Guid id)
    {
        var halls = await _hallOwnerService.GetHallOwnerHallsAsync(id);
        return Ok(halls);
    }

    /// <summary>
    /// تحديث ملف صاحب الصالة الشخصي
    /// </summary>
    /// <param name="id">معرف صاحب الصالة</param>
    /// <param name="updateDto">بيانات التحديث</param>
    /// <returns>بيانات صاحب الصالة المُحدث</returns>
    [HttpPut("{id}/profile")]
    public async Task<ActionResult<HallOwnerReadDTO>> UpdateProfile(Guid id, [FromBody] HallOwnerUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var owner = await _hallOwnerService.UpdateProfileAsync(id, updateDto);
        if (owner == null)
            return NotFound($"لم يتم العثور على صاحب صالة بالمعرف: {id}");

        return Ok(owner);
    }

    /// <summary>
    /// الحصول على إحصائيات صاحب الصالة
    /// </summary>
    /// <param name="id">معرف صاحب الصالة</param>
    /// <returns>كائن إحصائيات</returns>
    [HttpGet("{id}/statistics")]
    public async Task<ActionResult<object>> GetStatistics(Guid id)
    {
        var statistics = await _hallOwnerService.GetOwnerStatisticsAsync(id);
        return Ok(statistics);
    }
}
