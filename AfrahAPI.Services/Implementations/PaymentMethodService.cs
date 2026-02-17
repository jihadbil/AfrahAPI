using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.PaymentMethod;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة طرق الدفع
/// </summary>
public class PaymentMethodService : BaseService<PaymentMethod, PaymentMethodCreateDTO, PaymentMethodReadDTO, PaymentMethodUpdateDTO>, IPaymentMethodService
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPaymentMethodRepository paymentMethodRepository)
        : base(unitOfWork, mapper, paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<IEnumerable<PaymentMethodReadDTO>> GetActivePaymentMethodsAsync()
    {
        var methods = await _paymentMethodRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PaymentMethodReadDTO>>(methods);
    }

    public override async Task<PaymentMethodReadDTO> CreateAsync(PaymentMethodCreateDTO createDto)
    {
        var method = _mapper.Map<PaymentMethod>(createDto);
        method.MethodId = Guid.NewGuid();

        await _paymentMethodRepository.AddAsync(method);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<PaymentMethodReadDTO>(method);
    }
}
