using AfrahAPI.Models.DTOs.HallServices;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة خدمات الصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallServicesController : ControllerBase
{
    private readonly IHallServicesService _hallServicesService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallServicesController(IHallServicesService hallServicesService)
    {
        _hallServicesService = hallServicesService;
    }

    /// <summary>
    /// الحصول على جميع الخدمات
    /// </summary>
    /// <returns>قائمة بجميع الخدمات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallServicesReadDTO>>> GetAll()
    {
        var services = await _hallServicesService.GetAllAsync();
        return Ok(services);
    }

    /// <summary>
    /// الحصول على خدمة بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف الخدمة</param>
    /// <returns>بيانات الخدمة</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallServicesReadDTO>> GetById(Guid id)
    {
        var service = await _hallServicesService.GetByIdAsync(id);
        if (service == null)
            return NotFound($"لم يتم العثور على خدمة بالمعرف: {id}");

        return Ok(service);
    }

    /// <summary>
    /// إنشاء خدمة جديدة
    /// </summary>
    /// <param name="createDto">بيانات الخدمة الجديدة</param>
    /// <returns>بيانات الخدمة المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<HallServicesReadDTO>> Create([FromBody] HallServicesCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var service = await _hallServicesService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = service.ServiceId }, service);
    }

    /// <summary>
    /// تحديث بيانات خدمة موجودة
    /// </summary>
    /// <param name="id">معرف الخدمة</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الخدمة المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallServicesReadDTO>> Update(Guid id, [FromBody] HallServicesUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var service = await _hallServicesService.UpdateAsync(id, updateDto);
        if (service == null)
            return NotFound($"لم يتم العثور على خدمة بالمعرف: {id}");

        return Ok(service);
    }

    /// <summary>
    /// حذف خدمة
    /// </summary>
    /// <param name="id">معرف الخدمة</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallServicesService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على خدمة بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على جميع خدمات صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالخدمات</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<HallServicesReadDTO>>> GetByHall(Guid hallId)
    {
        var services = await _hallServicesService.GetServicesByHallIdAsync(hallId);
        return Ok(services);
    }

    /// <summary>
    /// إضافة خدمة لصالة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="createDto">بيانات الخدمة</param>
    /// <returns>بيانات الخدمة المُنشأة</returns>
    [HttpPost("hall/{hallId}/add")]
    public async Task<ActionResult<HallServicesReadDTO>> AddToHall(Guid hallId, [FromBody] HallServicesCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var service = await _hallServicesService.AddServiceToHallAsync(hallId, createDto);
            return CreatedAtAction(nameof(GetById), new { id = service.ServiceId }, service);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
