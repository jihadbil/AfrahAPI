using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AfrahAPI.DataAccess.Seeders;

/// <summary>
/// الكلاس الرئيسي لإدارة جميع عمليات Seeding
/// </summary>
public class DbSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public DbSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// تنفيذ جميع عمليات Seeding بالترتيب الصحيح
    /// </summary>
    public async Task SeedAllAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<Guid>>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        try
        {
            Console.WriteLine("🌱 بدء عملية Seeding...");

            // 1. البيانات الأساسية (Payment Methods, Categories)
            Console.WriteLine("📊 إنشاء البيانات الأساسية...");
            var masterDataSeeder = new MasterDataSeeder(context);
            await masterDataSeeder.SeedAsync();
            Console.WriteLine("✅ تم إنشاء البيانات الأساسية بنجاح");

            // 2. المستخدمين والأدوار
            Console.WriteLine("👥 إنشاء المستخدمين والأدوار...");
            var identitySeeder = new IdentitySeeder(context, userManager, roleManager);
            await identitySeeder.SeedAsync();
            Console.WriteLine("✅ تم إنشاء المستخدمين بنجاح");

            // 3. الصالات والخدمات
            Console.WriteLine("🏛️ إنشاء الصالات والخدمات...");
            var hallSeeder = new HallSeeder(context);
            await hallSeeder.SeedAsync();
            Console.WriteLine("✅ تم إنشاء الصالات بنجاح");

            // 4. البيانات المعاملاتية
            Console.WriteLine("💰 إنشاء الحجوزات والفواتير...");
            var transactionalSeeder = new TransactionalDataSeeder(context);
            await transactionalSeeder.SeedAsync();
            Console.WriteLine("✅ تم إنشاء البيانات المعاملاتية بنجاح");

            Console.WriteLine("🎉 اكتملت عملية Seeding بنجاح!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ حدث خطأ أثناء Seeding: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// حذف جميع البيانات (استخدم بحذر!)
    /// </summary>
    public async Task ClearAllDataAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Console.WriteLine("⚠️ جاري حذف جميع البيانات...");

        // احذف بالترتيب العكسي للعلاقات
        context.Notifications.RemoveRange(context.Notifications);
        context.ServiceRatings.RemoveRange(context.ServiceRatings);
        context.HallRatings.RemoveRange(context.HallRatings);
        context.Payments.RemoveRange(context.Payments);
        context.InvoiceItems.RemoveRange(context.InvoiceItems);
        context.Invoices.RemoveRange(context.Invoices);
        context.Bookings.RemoveRange(context.Bookings);
        context.HallUnavailableDates.RemoveRange(context.HallUnavailableDates);
        context.HallPaymentMethods.RemoveRange(context.HallPaymentMethods);
        context.ServiceRatingSummaries.RemoveRange(context.ServiceRatingSummaries);
        context.HallServices.RemoveRange(context.HallServices);
        context.HallRatingSummaries.RemoveRange(context.HallRatingSummaries);
        context.Halls.RemoveRange(context.Halls);
        context.Employees.RemoveRange(context.Employees);
        context.Customers.RemoveRange(context.Customers);
        context.HallOwners.RemoveRange(context.HallOwners);
        context.PaymentMethods.RemoveRange(context.PaymentMethods);
        context.HallCategories.RemoveRange(context.HallCategories);

        await context.SaveChangesAsync();
        Console.WriteLine("✅ تم حذف جميع البيانات");
    }
}
