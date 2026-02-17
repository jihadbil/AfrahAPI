using AfrahAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess.Seeders;

/// <summary>
/// مُنشئ البيانات المعاملاتية (الحجوزات، الفواتير، المدفوعات)
/// </summary>
public class TransactionalDataSeeder
{
    private readonly ApplicationDbContext _context;
    private List<Guid> _hallIds = new();
    private List<Guid> _customerIds = new();
    private List<Guid> _paymentMethodIds = new();

    public TransactionalDataSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// إنشاء البيانات المعاملاتية
    /// </summary>
    public async Task SeedAsync()
    {
        // جلب البيانات الأساسية
        _hallIds = await _context.Halls.Select(h => h.HallID).ToListAsync();
        _customerIds = await _context.Customers.Select(c => c.CustomerID).ToListAsync();
        _paymentMethodIds = await _context.PaymentMethods.Select(p => p.MethodId).ToListAsync();

        if (!_hallIds.Any() || !_customerIds.Any())
            return;

        // 1. إنشاء الحجوزات
        var bookingIds = await SeedBookingsAsync();

        // 2. إنشاء الفواتير
        var invoiceIds = await SeedInvoicesAsync(bookingIds);

        // 3. إنشاء المدفوعات
        await SeedPaymentsAsync(invoiceIds);

        // 4. إنشاء التقييمات
        await SeedRatingsAsync();

        // 5. إنشاء الإشعارات
        await SeedNotificationsAsync();

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// إنشاء حجوزات تجريبية
    /// </summary>
    private async Task<List<Guid>> SeedBookingsAsync()
    {
        if (await _context.Bookings.AnyAsync())
            return await _context.Bookings.Select(b => b.BookingId).ToListAsync();

        var bookings = new List<Booking>();
        var random = new Random();
        string[] statuses = { "Pending", "Confirmed", "Cancelled", "Completed" };

        for (int i = 0; i < 10; i++)
        {
            var eventDate = DateTime.UtcNow.AddDays(random.Next(-30, 60));
            
            bookings.Add(new Booking
            {
                BookingId = Guid.NewGuid(),
                CustomerId = _customerIds[random.Next(_customerIds.Count)],
                HallId = _hallIds[random.Next(_hallIds.Count)],
                StartDate = eventDate,
                EndDate = eventDate.AddDays(1),
                EventType = i % 3 == 0 ? "حفل زفاف" : i % 3 == 1 ? "مؤتمر" : "احتفال",
                NumberOfGuests = random.Next(50, 500),
                Status = statuses[random.Next(statuses.Length)],
                TotalPrice = random.Next(10000, 50000),
                Notes = $"حجز رقم {i + 1}",
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 90))
            });
        }

        await _context.Bookings.AddRangeAsync(bookings);
        await _context.SaveChangesAsync();

        return bookings.Select(b => b.BookingId).ToList();
    }

    /// <summary>
    /// إنشاء فواتير الحجوزات
    /// </summary>
    private async Task<List<Guid>> SeedInvoicesAsync(List<Guid> bookingIds)
    {
        if (await _context.Invoices.AnyAsync())
            return await _context.Invoices.Select(i => i.InvoiceId).ToListAsync();

        var invoices = new List<Invoice>();
        var random = new Random();
        string[] statuses = { "Pending", "Paid", "Cancelled", "Overdue" };

        foreach (var bookingId in bookingIds)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) continue;

            var invoice = new Invoice
            {
                InvoiceId = Guid.NewGuid(),
                BookingId = bookingId,
                DueDate = DateTime.UtcNow.AddDays(random.Next(1, 30)),
                TotalAmount = booking.TotalPrice,
                Status = statuses[random.Next(statuses.Length)],
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                InvoiceNumber = $"INV{random.Next(1000, 9999)}"
            };

            invoices.Add(invoice);

            // إنشاء بنود الفاتورة
            var services = await _context.HallServices
                .Where(s => s.HallID == booking.HallId)
                .Take(3)
                .ToListAsync();

            var invoiceItems = new List<InvoiceItems>();
            foreach (var service in services)
            {
                invoiceItems.Add(new InvoiceItems
                {
                    InvoiceId = invoice.InvoiceId,
                    HallServiceId = service.ServiceId,
                    ItemType = "خدمة",
                    Quantity = 1,
                    UnitPrice = service.Price,
                    Total = service.Price
                });
            }

            await _context.InvoiceItems.AddRangeAsync(invoiceItems);
        }

        await _context.Invoices.AddRangeAsync(invoices);
        await _context.SaveChangesAsync();

        return invoices.Select(i => i.InvoiceId).ToList();
    }

    /// <summary>
    /// إنشاء المدفوعات
    /// </summary>
    private async Task SeedPaymentsAsync(List<Guid> invoiceIds)
    {
        if (await _context.Payments.AnyAsync())
            return;

        var payments = new List<Payment>();
        var random = new Random();

        foreach (var invoiceId in invoiceIds.Take(7)) // مدفوعات لبعض الفواتير فقط
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null) continue;

            payments.Add(new Payment
            {
                PaymentId = Guid.NewGuid(),
                InvoiceId = invoiceId,
                MethodId = _paymentMethodIds[random.Next(_paymentMethodIds.Count)],
                Amount = invoice.TotalAmount,
                Status = "Completed",
                ReferenceNumber = $"PAY{random.Next(100000, 999999)}",
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
            });
        }

        await _context.Payments.AddRangeAsync(payments);
    }

    /// <summary>
    /// إنشاء تقييمات تجريبية
    /// </summary>
    private async Task SeedRatingsAsync()
    {
        if (await _context.HallRatings.AnyAsync())
            return;

        var hallRatings = new List<HallRating>();
        var serviceRatings = new List<ServiceRating>();
        var random = new Random();

        // تقييمات الصالات
        foreach (var hallId in _hallIds.Take(3))
        {
            foreach (var customerId in _customerIds.Take(2))
            {
                hallRatings.Add(new HallRating
                {
                    RatingID = Guid.NewGuid(),
                    HallID = hallId,
                    CustomerID = customerId,
                    OverallRating = random.Next(3, 6),
                    Comment = "تجربة رائعة، ننصح بالحجز",
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60))
                });
            }
        }

        // تقييمات الخدمات
        var services = await _context.HallServices.Take(5).ToListAsync();
        foreach (var service in services)
        {
            serviceRatings.Add(new ServiceRating
            {
                ServiceRatingID = Guid.NewGuid(),
                HallServiceID = service.ServiceId,
                CustomerID = _customerIds[random.Next(_customerIds.Count)],
                Rating = random.Next(3, 6),
                Comment = "خدمة ممتازة",
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60))
            });
        }

        await _context.HallRatings.AddRangeAsync(hallRatings);
        await _context.ServiceRatings.AddRangeAsync(serviceRatings);
    }

    /// <summary>
    /// إنشاء إشعارات تجريبية
    /// </summary>
    private async Task SeedNotificationsAsync()
    {
        if (await _context.Notifications.AnyAsync())
            return;

        var notifications = new List<Notification>();

        foreach (var customerId in _customerIds)
        {
            notifications.Add(new Notification
            {
                NotificationID = Guid.NewGuid(),
                UserID = customerId,
                Title = "تم تأكيد حجزك",
                Message = "تم تأكيد حجزك بنجاح. شكراً لاختيارك خدماتنا!",
                IsRead = false,
                CreatedAt = DateTime.UtcNow.AddDays(-new Random().Next(1, 30))
            });
        }

        await _context.Notifications.AddRangeAsync(notifications);
    }
}
