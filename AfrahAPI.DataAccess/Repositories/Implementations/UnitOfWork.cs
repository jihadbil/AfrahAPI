using AfrahAPI.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AfrahAPI.DataAccess.Repositories.Implementations;

/// <summary>
/// تطبيق Unit of Work - لإدارة المعاملات وتنسيق العمل بين جميع الـ Repositories
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    #region Repository Properties

    public ICustomerRepository Customers { get; }
    public IHallOwnerRepository HallOwners { get; }
    public IHallRepository Halls { get; }
    public IHallCategorieRepository HallCategories { get; }
    public IHallMediaRepository HallMedias { get; }
    public IHallServicesRepository HallServices { get; }
    public IHallRatingRepository HallRatings { get; }
    public IHallRatingSummaryRepository HallRatingSummaries { get; }
    public IHallUnavailableDateRepository HallUnavailableDates { get; }
    public IHallPaymentMethodRepository HallPaymentMethods { get; }
    public IEmployeeRepository Employees { get; }
    public IBookingRepository Bookings { get; }
    public IInvoiceRepository Invoices { get; }
    public IInvoiceItemsRepository InvoiceItems { get; }
    public IPaymentMethodRepository PaymentMethods { get; }
    public IPaymentRepository Payments { get; }
    public IServiceRatingRepository ServiceRatings { get; }
    public IServiceRatingSummaryRepository ServiceRatingSummaries { get; }
    public INotificationRepository Notifications { get; }

    #endregion

    /// <summary>
    /// مُنشئ UnitOfWork
    /// </summary>
    /// <param name="context">سياق قاعدة البيانات</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        // تهيئة جميع الـ Repositories
        Customers = new CustomerRepository(_context);
        HallOwners = new HallOwnerRepository(_context);
        Halls = new HallRepository(_context);
        HallCategories = new HallCategorieRepository(_context);
        HallMedias = new HallMediaRepository(_context);
        HallServices = new HallServicesRepository(_context);
        HallRatings = new HallRatingRepository(_context);
        HallRatingSummaries = new HallRatingSummaryRepository(_context);
        HallUnavailableDates = new HallUnavailableDateRepository(_context);
        HallPaymentMethods = new HallPaymentMethodRepository(_context);
        Employees = new EmployeeRepository(_context);
        Bookings = new BookingRepository(_context);
        Invoices = new InvoiceRepository(_context);
        InvoiceItems = new InvoiceItemsRepository(_context);
        PaymentMethods = new PaymentMethodRepository(_context);
        Payments = new PaymentRepository(_context);
        ServiceRatings = new ServiceRatingRepository(_context);
        ServiceRatingSummaries = new ServiceRatingSummaryRepository(_context);
        Notifications = new NotificationRepository(_context);
    }

    /// <inheritdoc/>
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <inheritdoc/>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// تحرير الموارد
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
