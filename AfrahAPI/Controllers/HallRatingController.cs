using AfrahAPI.Models.DTOs.HallRating;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة تقييمات الصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallRatingController : ControllerBase
{
    private readonly IHallRatingService _hallRatingService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallRatingController(IHallRatingService hallRatingService)
    {
        _hallRatingService = hallRatingService;
    }

    /// <summary>
    /// الحصول على جميع التقييمات
    /// </summary>
    /// <returns>قائمة بجميع التقييمات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallRatingReadDTO>>> GetAll()
    {
        var ratings = await _hallRatingService.GetAllAsync();
        return Ok(ratings);
    }

    /// <summary>
    /// الحصول على تقييم بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف التقييم</param>
    /// <returns>بيانات التقييم</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallRatingReadDTO>> GetById(Guid id)
    {
        var rating = await _hallRatingService.GetByIdAsync(id);
        if (rating == null)
            return NotFound($"لم يتم العثور على تقييم بالمعرف: {id}");

        return Ok(rating);
    }

    /// <summary>
    /// إنشاء تقييم جديد
    /// </summary>
    /// <param name="createDto">بيانات التقييم الجديد</param>
    /// <returns>بيانات التقييم المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<HallRatingReadDTO>> Create([FromBody] HallRatingCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rating = await _hallRatingService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = rating.RatingID }, rating);
    }

    /// <summary>
    /// تحديث بيانات تقييم موجود
    /// </summary>
    /// <param name="id">معرف التقييم</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات التقييم المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallRatingReadDTO>> Update(Guid id, [FromBody] HallRatingUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rating = await _hallRatingService.UpdateAsync(id, updateDto);
        if (rating == null)
            return NotFound($"لم يتم العثور على تقييم بالمعرف: {id}");

        return Ok(rating);
    }

    /// <summary>
    /// حذف تقييم
    /// </summary>
    /// <param name="id">معرف التقييم</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallRatingService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على تقييم بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على تقييمات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالتقييمات</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<HallRatingReadDTO>>> GetByHall(Guid hallId)
    {
        var ratings = await _hallRatingService.GetRatingsByHallAsync(hallId);
        return Ok(ratings);
    }

    /// <summary>
    /// إضافة تقييم جديد للصالة
    /// </summary>
    /// <param name="createDto">بيانات التقييم</param>
    /// <returns>بيانات التقييم المُنشأ</returns>
    [HttpPost("add")]
    public async Task<ActionResult<HallRatingReadDTO>> AddRating([FromBody] HallRatingCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var rating = await _hallRatingService.AddRatingAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = rating.RatingID }, rating);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// حساب متوسط التقييم للصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>متوسط التقييم</returns>
    [HttpGet("hall/{hallId}/average")]
    public async Task<ActionResult<decimal>> GetAverageRating(Guid hallId)
    {
        var average = await _hallRatingService.CalculateAverageRatingAsync(hallId);
        return Ok(new { hallId, averageRating = average });
    }
}
