using AfrahAPI.Models.DTOs.HallMedia;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة وسائط الصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallMediaController : ControllerBase
{
    private readonly IHallMediaService _hallMediaService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallMediaController(IHallMediaService hallMediaService)
    {
        _hallMediaService = hallMediaService;
    }

    /// <summary>
    /// الحصول على جميع الوسائط
    /// </summary>
    /// <returns>قائمة بجميع الوسائط</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallMediaReadDTO>>> GetAll()
    {
        var media = await _hallMediaService.GetAllAsync();
        return Ok(media);
    }

    /// <summary>
    /// الحصول على وسائط بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف الوسائط</param>
    /// <returns>بيانات الوسائط</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallMediaReadDTO>> GetById(Guid id)
    {
        var media = await _hallMediaService.GetByIdAsync(id);
        if (media == null)
            return NotFound($"لم يتم العثور على وسائط بالمعرف: {id}");

        return Ok(media);
    }

    /// <summary>
    /// إنشاء وسائط جديدة
    /// </summary>
    /// <param name="createDto">بيانات الوسائط الجديدة</param>
    /// <returns>بيانات الوسائط المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<HallMediaReadDTO>> Create([FromBody] HallMediaCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var media = await _hallMediaService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = media.MediaID }, media);
    }

    /// <summary>
    /// تحديث بيانات وسائط موجودة
    /// </summary>
    /// <param name="id">معرف الوسائط</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الوسائط المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallMediaReadDTO>> Update(Guid id, [FromBody] HallMediaUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var media = await _hallMediaService.UpdateAsync(id, updateDto);
        if (media == null)
            return NotFound($"لم يتم العثور على وسائط بالمعرف: {id}");

        return Ok(media);
    }

    /// <summary>
    /// حذف وسائط
    /// </summary>
    /// <param name="id">معرف الوسائط</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallMediaService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على وسائط بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على جميع الوسائط لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالوسائط</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<HallMediaReadDTO>>> GetByHall(Guid hallId)
    {
        var media = await _hallMediaService.GetMediaByHallIdAsync(hallId);
        return Ok(media);
    }

    /// <summary>
    /// رفع وسائط جديدة لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="createDto">بيانات الوسائط</param>
    /// <returns>بيانات الوسائط المُنشأة</returns>
    [HttpPost("hall/{hallId}/upload")]
    public async Task<ActionResult<HallMediaReadDTO>> Upload(Guid hallId, [FromBody] HallMediaCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var media = await _hallMediaService.UploadMediaAsync(hallId, createDto);
            return CreatedAtAction(nameof(GetById), new { id = media.MediaID }, media);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
