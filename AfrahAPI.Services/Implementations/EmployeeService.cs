using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Employee;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة الموظفين
/// </summary>
public class EmployeeService : BaseService<Employee, EmployeeCreateDTO, EmployeeReadDTO, EmployeeUpdateDTO>, IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmployeeRepository employeeRepository)
        : base(unitOfWork, mapper, employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeReadDTO>> GetEmployeesByHallAsync(Guid hallId)
    {
        var employees = await _employeeRepository.FindAsync(e => e.HallID == hallId);
        return _mapper.Map<IEnumerable<EmployeeReadDTO>>(employees);
    }

    public override async Task<EmployeeReadDTO> CreateAsync(EmployeeCreateDTO createDto)
    {
        var employee = _mapper.Map<Employee>(createDto);
        employee.EmployeeId = Guid.NewGuid();
        employee.HireDate = DateTime.UtcNow;
        employee.CreatedAt = DateTime.UtcNow;

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EmployeeReadDTO>(employee);
    }
}
