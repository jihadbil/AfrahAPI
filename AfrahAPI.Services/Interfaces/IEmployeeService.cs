using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Employee;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة الموظفين
/// </summary>
public interface IEmployeeService : IBaseService<Employee, EmployeeCreateDTO, EmployeeReadDTO, EmployeeUpdateDTO>
{
    /// <summary>
    /// الحصول على موظفي صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالموظفين</returns>
    Task<IEnumerable<EmployeeReadDTO>> GetEmployeesByHallAsync(Guid hallId);
}
