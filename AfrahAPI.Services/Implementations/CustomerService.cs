using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Customer;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة العملاء
/// </summary>
public class CustomerService : BaseService<Customer, CustomerCreateDTO, CustomerReadDTO, CustomerUpdateDTO>, ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// المُنشئ
    /// </summary>
    public CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICustomerRepository customerRepository)
        : base(unitOfWork, mapper, customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// الحصول على عميل بواسطة معرف المستخدم المرتبط
    /// </summary>
    public async Task<CustomerReadDTO?> GetCustomerByUserIdAsync(Guid userId)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(c => c.UserID == userId);
        return customer != null ? _mapper.Map<CustomerReadDTO>(customer) : null;
    }

    /// <summary>
    /// الحصول على جميع حجوزات العميل
    /// </summary>
    public async Task<IEnumerable<object>> GetCustomerBookingsAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return Enumerable.Empty<object>();

        // سيتم تنفيذ هذا بشكل كامل بعد إنشاء خدمة الحجوزات
        return _mapper.Map<IEnumerable<object>>(customer.Bookings);
    }

    /// <summary>
    /// تحديث ملف العميل الشخصي
    /// </summary>
    public async Task<CustomerReadDTO?> UpdateProfileAsync(Guid customerId, CustomerUpdateDTO updateDto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return null;

        // تحديث الحقول
        _mapper.Map(updateDto, customer);
        customer.UpdatedAt = DateTime.UtcNow;

        _customerRepository.Update(customer);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<CustomerReadDTO>(customer);
    }

    /// <summary>
    /// إنشاء عميل جديد مع تعيين وقت الإنشاء
    /// </summary>
    public override async Task<CustomerReadDTO> CreateAsync(CustomerCreateDTO createDto)
    {
        var customer = _mapper.Map<Customer>(createDto);
        customer.CreatedAt = DateTime.UtcNow;
        customer.CustomerID = Guid.NewGuid();

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<CustomerReadDTO>(customer);
    }
}
