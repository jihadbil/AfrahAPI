using AfrahAPI.Models.DTOs.Employee;
using AfrahAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfrahAPI.Controllers;

/// <summary>
/// Controller لإدارة الموظفين
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    /// <summary>
    /// Constructor
    /// </summary>
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// الحصول على جميع الموظفين
    /// </summary>
    /// <returns>قائمة بجميع الموظفين</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeReadDTO>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    /// <summary>
    /// الحصول على موظف بواسطة معرفه
    /// </summary>
    /// <param name="id">معرف الموظف</param>
    /// <returns>بيانات الموظف</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeReadDTO>> GetById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound($"لم يتم العثور على موظف بالمعرف: {id}");

        return Ok(employee);
    }

    /// <summary>
    /// إنشاء موظف جديد
    /// </summary>
    /// <param name="createDto">بيانات الموظف الجديد</param>
    /// <returns>بيانات الموظف المُنشأ</returns>
    [HttpPost]
    public async Task<ActionResult<EmployeeReadDTO>> Create([FromBody] EmployeeCreateDTO createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = await _employeeService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = employee.EmployeeId }, employee);
    }

    /// <summary>
    /// تحديث بيانات موظف موجود
    /// </summary>
    /// <param name="id">معرف الموظف</param>
    /// <param name="updateDto">البيانات المُحدثة</param>
    /// <returns>بيانات الموظف المُحدث</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeReadDTO>> Update(Guid id, [FromBody] EmployeeUpdateDTO updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = await _employeeService.UpdateAsync(id, updateDto);
        if (employee == null)
            return NotFound($"لم يتم العثور على موظف بالمعرف: {id}");

        return Ok(employee);
    }

    /// <summary>
    /// حذف موظف
    /// </summary>
    /// <param name="id">معرف الموظف</param>
    /// <returns>نتيجة العملية</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _employeeService.DeleteAsync(id);
        if (!result)
            return NotFound($"لم يتم العثور على موظف بالمعرف: {id}");

        return NoContent();
    }

    /// <summary>
    /// الحصول على موظفي صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالموظفين</returns>
    [HttpGet("hall/{hallId}")]
    public async Task<ActionResult<IEnumerable<EmployeeReadDTO>>> GetByHall(Guid hallId)
    {
        var employees = await _employeeService.GetEmployeesByHallAsync(hallId);
        return Ok(employees);
    }
}
