using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Payment;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة المدفوعات
/// </summary>
public class PaymentService : BaseService<Payment, PaymentCreateDTO, PaymentReadDTO, PaymentUpdateDTO>, IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public PaymentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPaymentRepository paymentRepository,
        IInvoiceRepository invoiceRepository)
        : base(unitOfWork, mapper, paymentRepository)
    {
        _paymentRepository = paymentRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<PaymentReadDTO> ProcessPaymentAsync(PaymentCreateDTO createDto)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(createDto.InvoiceId);
        if (invoice == null)
            throw new Exception("الفاتورة غير موجودة");

        var payment = _mapper.Map<Payment>(createDto);
        payment.PaymentId = Guid.NewGuid();
        payment.PaymentDate = DateTime.UtcNow;
        payment.Status = "Completed"; // يمكن تغييرها حسب منطق المعالجة

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<PaymentReadDTO>(payment);
    }

    public async Task<IEnumerable<PaymentReadDTO>> GetPaymentsByInvoiceAsync(Guid invoiceId)
    {
        var payments = await _paymentRepository.FindAsync(p => p.InvoiceId == invoiceId);
        return _mapper.Map<IEnumerable<PaymentReadDTO>>(payments);
    }

    public async Task<PaymentReadDTO?> RefundPaymentAsync(Guid paymentId, decimal amount)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            return null;

        if (payment.Status != "Completed")
            throw new Exception("لا يمكن استرداد مبلغ من دفعة غير مكتملة");

        if (amount > payment.Amount)
            throw new Exception("مبلغ الاسترداد أكبر من المبلغ المدفوع");

        payment.Status = "Refunded";
        payment.Amount = amount; // أو يمكن إنشاء سجل استرداد منفصل

        _paymentRepository.Update(payment);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<PaymentReadDTO>(payment);
    }

    public override async Task<PaymentReadDTO> CreateAsync(PaymentCreateDTO createDto)
    {
        var payment = _mapper.Map<Payment>(createDto);
        payment.PaymentId = Guid.NewGuid();
        payment.PaymentDate = DateTime.UtcNow;

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<PaymentReadDTO>(payment);
    }
}
