namespace AfrahAPI.DataAccess.Repositories.Interfaces;

/// <summary>
/// واجهة Unit of Work - لإدارة المعاملات وتنسيق العمل بين جميع الـ Repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    #region Repository Properties

    /// <summary>
    /// مستودع العملاء
    /// </summary>
    ICustomerRepository Customers { get; }

    /// <summary>
    /// مستودع أصحاب الصالات
    /// </summary>
    IHallOwnerRepository HallOwners { get; }

    /// <summary>
    /// مستودع الصالات
    /// </summary>
    IHallRepository Halls { get; }

    /// <summary>
    /// مستودع فئات الصالات
    /// </summary>
    IHallCategorieRepository HallCategories { get; }

    /// <summary>
    /// مستودع وسائط الصالات
    /// </summary>
    IHallMediaRepository HallMedias { get; }

    /// <summary>
    /// مستودع خدمات الصالات
    /// </summary>
    IHallServicesRepository HallServices { get; }

    /// <summary>
    /// مستودع تقييمات الصالات
    /// </summary>
    IHallRatingRepository HallRatings { get; }

    /// <summary>
    /// مستودع ملخصات تقييمات الصالات
    /// </summary>
    IHallRatingSummaryRepository HallRatingSummaries { get; }

    /// <summary>
    /// مستودع التواريخ غير المتاحة
    /// </summary>
    IHallUnavailableDateRepository HallUnavailableDates { get; }

    /// <summary>
    /// مستودع طرق الدفع للصالات
    /// </summary>
    IHallPaymentMethodRepository HallPaymentMethods { get; }

    /// <summary>
    /// مستودع الموظفين
    /// </summary>
    IEmployeeRepository Employees { get; }

    /// <summary>
    /// مستودع الحجوزات
    /// </summary>
    IBookingRepository Bookings { get; }

    /// <summary>
    /// مستودع الفواتير
    /// </summary>
    IInvoiceRepository Invoices { get; }

    /// <summary>
    /// مستودع بنود الفاتورة
    /// </summary>
    IInvoiceItemsRepository InvoiceItems { get; }

    /// <summary>
    /// مستودع وسائل الدفع
    /// </summary>
    IPaymentMethodRepository PaymentMethods { get; }

    /// <summary>
    /// مستودع المدفوعات
    /// </summary>
    IPaymentRepository Payments { get; }

    /// <summary>
    /// مستودع تقييمات الخدمات
    /// </summary>
    IServiceRatingRepository ServiceRatings { get; }

    /// <summary>
    /// مستودع ملخصات تقييمات الخدمات
    /// </summary>
    IServiceRatingSummaryRepository ServiceRatingSummaries { get; }

    /// <summary>
    /// مستودع الإشعارات
    /// </summary>
    INotificationRepository Notifications { get; }

    #endregion

    /// <summary>
    /// حفظ جميع التغييرات في قاعدة البيانات
    /// </summary>
    /// <returns>عدد السجلات المتأثرة</returns>
    Task<int> CompleteAsync();

    /// <summary>
    /// بدء معاملة جديدة
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// تأكيد المعاملة الحالية
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// التراجع عن المعاملة الحالية
    /// </summary>
    Task RollbackTransactionAsync();
}
