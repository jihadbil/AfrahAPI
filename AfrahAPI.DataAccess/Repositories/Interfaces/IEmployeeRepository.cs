using AfrahAPI.Models;

namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة مستودع الموظفين - توفر عمليات متخصصة للموظفين
/// </summary>
public interface IEmployeeRepository : IRepository<Employee>
{
    /// <summary>
    /// جلب جميع الموظفين التابعين لصالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالموظفين</returns>
    Task<IEnumerable<Employee>> GetEmployeesByHallIdAsync(Guid hallId);

    /// <summary>
    /// جلب الموظفين حسب المسمى الوظيفي
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <param name="jobTitle">المسمى الوظيفي</param>
    /// <returns>قائمة بالموظفين</returns>
    Task<IEnumerable<Employee>> GetEmployeesByJobTitleAsync(Guid hallId, string jobTitle);

    /// <summary>
    /// البحث عن الموظفين بالاسم
    /// </summary>
    /// <param name="name">الاسم المراد البحث عنه</param>
    /// <returns>قائمة بالموظفين المطابقين</returns>
    Task<IEnumerable<Employee>> SearchEmployeesByNameAsync(string name);

    /// <summary>
    /// جلب الموظفين النشطين فقط
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>قائمة بالموظفين النشطين</returns>
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync(Guid hallId);

    /// <summary>
    /// جلب الموظفين حسب نطاق الراتب
    /// </summary>
    /// <param name="minSalary">الحد الأدنى للراتب</param>
    /// <param name="maxSalary">الحد الأقصى للراتب</param>
    /// <returns>قائمة بالموظفين</returns>
    Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary);

    /// <summary>
    /// عد عدد الموظفين في صالة معينة
    /// </summary>
    /// <param name="hallId">معرف الصالة</param>
    /// <returns>عدد الموظفين</returns>
    Task<int> GetEmployeesCountByHallIdAsync(Guid hallId);
}
