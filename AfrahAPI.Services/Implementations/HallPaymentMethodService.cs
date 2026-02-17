using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.HallPaymentMethod;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة طرق الدفع للصالات
/// </summary>
public class HallPaymentMethodService : BaseService<HallPaymentMethod, HallPaymentMethodCreateDTO, HallPaymentMethodReadDTO, HallPaymentMethodUpdateDTO>, IHallPaymentMethodService
{
    private readonly IHallPaymentMethodRepository _hallPaymentMethodRepository;
    private readonly IHallRepository _hallRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public HallPaymentMethodService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHallPaymentMethodRepository hallPaymentMethodRepository,
        IHallRepository hallRepository,
        IPaymentMethodRepository paymentMethodRepository)
        : base(unitOfWork, mapper, hallPaymentMethodRepository)
    {
        _hallPaymentMethodRepository = hallPaymentMethodRepository;
        _hallRepository = hallRepository;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<HallPaymentMethodReadDTO> AddPaymentMethodToHallAsync(Guid hallId, Guid paymentMethodId)
    {
        // التحقق من وجود الصالة
        var hall = await _hallRepository.GetByIdAsync(hallId);
        if (hall == null)
            throw new Exception("الصالة غير موجودة");

        // التحقق من وجود طريقة الدفع
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodId);
        if (paymentMethod == null)
            throw new Exception("طريقة الدفع غير موجودة");

        // التحقق من عدم وجود الربط مسبقاً
        var existing = await _hallPaymentMethodRepository.FirstOrDefaultAsync(
            hpm => hpm.HallID == hallId && hpm.PaymentMethodID == paymentMethodId);

        if (existing != null)
            throw new Exception("طريقة الدفع مضافة مسبقاً لهذه الصالة");

        var hallPaymentMethod = new HallPaymentMethod
        {
            HallPaymentMethodID = Guid.NewGuid(),
            HallID = hallId,
            PaymentMethodID = paymentMethodId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _hallPaymentMethodRepository.AddAsync(hallPaymentMethod);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallPaymentMethodReadDTO>(hallPaymentMethod);
    }

    public async Task<bool> RemovePaymentMethodFromHallAsync(Guid hallId, Guid paymentMethodId)
    {
        var hallPaymentMethod = await _hallPaymentMethodRepository.FirstOrDefaultAsync(
            hpm => hpm.HallID == hallId && hpm.PaymentMethodID == paymentMethodId);

        if (hallPaymentMethod == null)
            return false;

        _hallPaymentMethodRepository.Remove(hallPaymentMethod);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<IEnumerable<HallPaymentMethodReadDTO>> GetHallPaymentMethodsAsync(Guid hallId)
    {
        var hallPaymentMethods = await _hallPaymentMethodRepository.FindAsync(
            hpm => hpm.HallID == hallId && hpm.IsActive);
        
        return _mapper.Map<IEnumerable<HallPaymentMethodReadDTO>>(hallPaymentMethods);
    }

    public async Task<IEnumerable<HallPaymentMethodReadDTO>> GetHallsByPaymentMethodAsync(Guid paymentMethodId)
    {
        var hallPaymentMethods = await _hallPaymentMethodRepository.FindAsync(
            hpm => hpm.PaymentMethodID == paymentMethodId && hpm.IsActive);
        
        return _mapper.Map<IEnumerable<HallPaymentMethodReadDTO>>(hallPaymentMethods);
    }

    public override async Task<HallPaymentMethodReadDTO> CreateAsync(HallPaymentMethodCreateDTO createDto)
    {
        var hallPaymentMethod = _mapper.Map<HallPaymentMethod>(createDto);
        hallPaymentMethod.HallPaymentMethodID = Guid.NewGuid();
        hallPaymentMethod.CreatedAt = DateTime.UtcNow;

        await _hallPaymentMethodRepository.AddAsync(hallPaymentMethod);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<HallPaymentMethodReadDTO>(hallPaymentMethod);
    }
}
