using AfrahAPI.Models.DTOs.ServiceRating;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة تقييمات الخدمات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ServiceRatingController : ControllerBase
{
    private readonly IServiceRatingService _serviceRatingService;

    /// <summary>
    /// Constructor
    /// </summary>
    public ServiceRatingController(IServiceRatingService serviceRatingService)
    {
        _serviceRatingService = serviceRatingService;
    }

    /// <summary>
    /// الحصول على جميع التقييمات
    /// </summary>
    /// <returns>قائمة بجميع التقييمات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceRatingReadDTO>>> GetAll()
    {
        var ratings = await _serviceRatingService.GetAllAsync();
        return Ok(ratings);
    }

    /// <summary>
    /// الحصول على تقييم بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف التقييم</param>
    /// <returns>بيانات التقييم</returns>
    [HttpGet("{ id}")]
    public async Task<ActionResult<ServiceRatingReadDTO>> GetById(Guid id)
    {
        var rating = await _serviceRatingService.GetByIdAsync(id);
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
    public async Task<ActionResult<ServiceRatingReadDTO>> Create([FromBody] ServiceRatingCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rating = await _serviceRatingService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = rating.ServiceRatingID }, rating);
    }

    /// <summary>
    /// تحديث بيانات تقييم موجود
    /// </summary>
    /// <param name="id">معرف التقييم</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات التقييم المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceRatingReadDTO>> Update(Guid id, [FromBody] ServiceRatingUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rating = await _serviceRatingService.UpdateAsync(id, updateDto);
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
        var result = await _serviceRatingService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على تقييم بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على تقييمات خدمة معينة
    /// </summary>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <returns>قائمة بالتقييمات</returns>
    [HttpGet("service/{serviceId}")]
    public async Task<ActionResult<IEnumerable<ServiceRatingReadDTO>>> GetByService(Guid serviceId)
    {
        var ratings = await _serviceRatingService.GetRatingsByServiceAsync(serviceId);
        return Ok(ratings);
    }

    /// <summary>
    /// إضافة تقييم جديد للخدمة
    /// </summary>
    /// <param name="createDto">بيانات التقييم</param>
    /// <returns>بيانات التقييم المُنشأ</returns>
    [HttpPost("add")]
    public async Task<ActionResult<ServiceRatingReadDTO>> AddRating([FromBody] ServiceRatingCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var rating = await _serviceRatingService.AddRatingAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = rating.ServiceRatingID }, rating);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// حساب متوسط التقييم للخدمة
    /// </summary>
    /// <param name="serviceId">معرف الخدمة</param>
    /// <returns>متوسط التقييم</returns>
    [HttpGet("service/{serviceId}/average")]
    public async Task<ActionResult<decimal>> GetAverageRating(Guid serviceId)
    {
        var average = await _serviceRatingService.CalculateAverageRatingAsync(serviceId);
        return Ok(new { serviceId, averageRating = average });
    }
}
