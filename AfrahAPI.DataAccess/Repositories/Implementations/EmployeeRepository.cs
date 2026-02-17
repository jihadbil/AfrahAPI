using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق مستودع الموظفين
/// </summary>
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    /// <summary>
    /// مُنشئ EmployeeRepository
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public EmployeeRepository(ApplicationDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IEnumerable<Employee>> GetEmployeesByHallIdAsync(Guid hallId) =>
        await _dbSet.Where(e => e.HallID == hallId).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Employee>> GetEmployeesByJobTitleAsync(Guid hallId, string jobTitle) =>
        await _dbSet.Where(e => e.HallID == hallId && e.JobTitle == jobTitle).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Employee>> SearchEmployeesByNameAsync(string name) =>
        await _dbSet.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name)).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync(Guid hallId) =>
        await _dbSet.Where(e => e.HallID == hallId).ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary) =>
        await _dbSet.Where(e => e.Salary >= minSalary && e.Salary <= maxSalary).ToListAsync();

    /// <inheritdoc/>
    public async Task<int> GetEmployeesCountByHallIdAsync(Guid hallId) =>
        await _dbSet.CountAsync(e => e.HallID == hallId);
}
