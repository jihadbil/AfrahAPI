using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Invoice;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة الفواتير
/// </summary>
public class InvoiceService : BaseService<Invoice, InvoiceCreateDTO, InvoiceReadDTO, InvoiceUpdateDTO>, IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IInvoiceItemsRepository _invoiceItemsRepository;

    public InvoiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IInvoiceRepository invoiceRepository,
        IBookingRepository bookingRepository,
        IInvoiceItemsRepository invoiceItemsRepository)
        : base(unitOfWork, mapper, invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
        _bookingRepository = bookingRepository;
        _invoiceItemsRepository = invoiceItemsRepository;
    }

    public async Task<InvoiceReadDTO> GenerateInvoiceForBookingAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking == null)
            throw new Exception("الحجز غير موجود");

        // التحقق من عدم وجود فاتورة مسبقة
        var existingInvoice = await _invoiceRepository.FirstOrDefaultAsync(i => i.BookingId == bookingId);
        if (existingInvoice != null)
            throw new Exception("يوجد فاتورة لهذا الحجز بالفعل");

        var invoice = new Invoice
        {
            InvoiceId = Guid.NewGuid(),
            BookingId = bookingId,
            invoiceDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7),
            TotalAmount = booking.TotalPrice * 1.15m, // مع الضريبة 15%
            PaidAmount = 0,
            BalanceDue = booking.TotalPrice * 1.15m,
            Status = "Pending"
        };

        await _invoiceRepository.AddAsync(invoice);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<InvoiceReadDTO>(invoice);
    }

    public async Task<IEnumerable<InvoiceReadDTO>> GetInvoicesByCustomerAsync(Guid customerId)
    {
        var bookings = await _bookingRepository.FindAsync(b => b.CustomerId == customerId);
        var bookingIds = bookings.Select(b => b.BookingId).ToList();

        var invoices = await _invoiceRepository.FindAsync(i => bookingIds.Contains(i.BookingId));
        return _mapper.Map<IEnumerable<InvoiceReadDTO>>(invoices);
    }

    public async Task<decimal> CalculateInvoiceTotalAsync(Guid invoiceId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
            throw new Exception("الفاتورة غير موجودة");

        var items = await _invoiceItemsRepository.FindAsync(i => i.InvoiceId == invoiceId);
        var itemsTotal = items.Sum(i => i.Total);

        return invoice.TotalAmount + itemsTotal;
    }

    public override async Task<InvoiceReadDTO> CreateAsync(InvoiceCreateDTO createDto)
    {
        var invoice = _mapper.Map<Invoice>(createDto);
        invoice.InvoiceId = Guid.NewGuid();
        invoice.invoiceDate = DateTime.UtcNow;

        await _invoiceRepository.AddAsync(invoice);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<InvoiceReadDTO>(invoice);
    }
}
