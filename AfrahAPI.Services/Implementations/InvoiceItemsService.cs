using AfrahAPI.DataAccess.Repositories.Interfaces;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.InvoiceItems;
using AfrahAPI.Services.Interfaces;
using AutoMapper;

namespace AfrahAPI.Services.Implementations;

/// <summary>
/// تطبيق خدمة إدارة بنود الفاتورة
/// </summary>
public class InvoiceItemsService : BaseService<InvoiceItems, InvoiceItemsCreateDTO, InvoiceItemsReadDTO, InvoiceItemsUpdateDTO>, IInvoiceItemsService
{
    private readonly IInvoiceItemsRepository _invoiceItemsRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceItemsService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IInvoiceItemsRepository invoiceItemsRepository,
        IInvoiceRepository invoiceRepository)
        : base(unitOfWork, mapper, invoiceItemsRepository)
    {
        _invoiceItemsRepository = invoiceItemsRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<IEnumerable<InvoiceItemsReadDTO>> GetItemsByInvoiceAsync(Guid invoiceId)
    {
        var items = await _invoiceItemsRepository.FindAsync(i => i.InvoiceId == invoiceId);
        return _mapper.Map<IEnumerable<InvoiceItemsReadDTO>>(items);
    }

    public async Task<InvoiceItemsReadDTO> AddItemToInvoiceAsync(Guid invoiceId, InvoiceItemsCreateDTO createDto)
    {
        // التحقق من وجود الفاتورة
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
            throw new Exception("الفاتورة غير موجودة");

        var item = _mapper.Map<InvoiceItems>(createDto);
        item.ItemId = Guid.NewGuid();
        item.InvoiceId = invoiceId;
        item.CreatedAt = DateTime.UtcNow;

        // حساب الإجمالي
        item.Total = item.Quantity * item.UnitPrice;

        await _invoiceItemsRepository.AddAsync(item);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<InvoiceItemsReadDTO>(item);
    }

    public async Task<bool> RemoveItemAsync(Guid itemId)
    {
        var item = await _invoiceItemsRepository.GetByIdAsync(itemId);
        if (item == null)
            return false;

        _invoiceItemsRepository.Remove(item);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<decimal> CalculateItemTotalAsync(Guid itemId)
    {
        var item = await _invoiceItemsRepository.GetByIdAsync(itemId);
        if (item == null)
            throw new Exception("البند غير موجود");

        return item.Quantity * item.UnitPrice;
    }

    public async Task<decimal> CalculateInvoiceItemsTotalAsync(Guid invoiceId)
    {
        var items = await _invoiceItemsRepository.FindAsync(i => i.InvoiceId == invoiceId);
        return items.Sum(i => i.Total);
    }

    public override async Task<InvoiceItemsReadDTO> CreateAsync(InvoiceItemsCreateDTO createDto)
    {
        var item = _mapper.Map<InvoiceItems>(createDto);
        item.ItemId = Guid.NewGuid();
        item.CreatedAt = DateTime.UtcNow;
        item.Total = item.Quantity * item.UnitPrice;

        await _invoiceItemsRepository.AddAsync(item);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<InvoiceItemsReadDTO>(item);
    }
}
