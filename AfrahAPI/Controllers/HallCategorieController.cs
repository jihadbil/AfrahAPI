using AfrahAPI.Models.DTOs.HallCategorie;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة فئات الصالات
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HallCategorieController : ControllerBase
{
    private readonly IHallCategorieService _hallCategorieService;

    /// <summary>
    /// Constructor
    /// </summary>
    public HallCategorieController(IHallCategorieService hallCategorieService)
    {
        _hallCategorieService = hallCategorieService;
    }

    /// <summary>
    /// الحصول على جميع الفئات
    /// </summary>
    /// <returns>قائمة بجميع الفئات</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HallCategorieReadDTO>>> GetAll()
    {
        var categories = await _hallCategorieService.GetAllAsync();
        return Ok(categories);
    }

    /// <summary>
    /// الحصول على فئة بواسطة معرفها
    /// </summary>
    /// <param name="id">معرف الفئة</param>
    /// <returns>بيانات الفئة</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<HallCategorieReadDTO>> GetById(Guid id)
    {
        var category = await _hallCategorieService.GetByIdAsync(id);
        if (category == null)
            return NotFound($"لم يتم العثور على فئة بالمعرف: {id}");

        return Ok(category);
    }

    /// <summary>
    /// إنشاء فئة جديدة
    /// </summary>
    /// <param name="createDto">بيانات الفئة الجديدة</param>
    /// <returns>بيانات الفئة المُنشأة</returns>
    [HttpPost]
    public async Task<ActionResult<HallCategorieReadDTO>> Create([FromBody] HallCategorieCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _hallCategorieService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryID }, category);
    }

    /// <summary>
    /// تحديث بيانات فئة موجودة
    /// </summary>
    /// <param name="id">معرف الفئة</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الفئة المُحدثة</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HallCategorieReadDTO>> Update(Guid id, [FromBody] HallCategorieUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _hallCategorieService.UpdateAsync(id, updateDto);
        if (category == null)
            return NotFound($"لم يتم العثور على فئة بالمعرف: {id}");

        return Ok(category);
    }

    /// <summary>
    /// حذف فئة
    /// </summary>
    /// <param name="id">معرف الفئة</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _hallCategorieService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على فئة بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على الفئة مع جميع صالاتها
    /// </summary>
    /// <param name="id">معرف الفئة</param>
    /// <returns>بيانات الفئة</returns>
    [HttpGet("{id}/with-halls")]
    public async Task<ActionResult<HallCategorieReadDTO>> GetWithHalls(Guid id)
    {
        var category = await _hallCategorieService.GetCategoryWithHallsAsync(id);
        if (category == null)
            return NotFound($"لم يتم العثور على فئة بالمعرف: {id}");

        return Ok(category);
    }
}
