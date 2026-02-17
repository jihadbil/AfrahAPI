using AfrahAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AfrahAPI.DataAccess;

/// <summary>
/// سياق قاعدة البيانات الرئيسي لنظام إدارة حجوزات قاعات الأفراح
/// </summary>
public class ApplicationDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    /// <summary>
    /// مُنشئ سياق قاعدة البيانات
    /// </summary>
    /// <param name="options">خيارات التكوين</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region DbSets - مجموعات الكيانات

    /// <summary>
    /// مجموعة العملاء
    /// </summary>
    public DbSet<Customer> Customers { get; set; }

    /// <summary>
    /// مجموعة أصحاب الصالات
    /// </summary>
    public DbSet<HallOwner> HallOwners { get; set; }

    /// <summary>
    /// مجموعة الصالات
    /// </summary>
    public DbSet<Hall> Halls { get; set; }

    /// <summary>
    /// مجموعة فئات الصالات
    /// </summary>
    public DbSet<HallCategorie> HallCategories { get; set; }

    /// <summary>
    /// مجموعة وسائط الصالات
    /// </summary>
    public DbSet<HallMedia> HallMedias { get; set; }

    /// <summary>
    /// مجموعة خدمات الصالات
    /// </summary>
    public DbSet<HallServices> HallServices { get; set; }

    /// <summary>
    /// مجموعة تقييمات الصالات
    /// </summary>
    public DbSet<HallRating> HallRatings { get; set; }

    /// <summary>
    /// مجموعة ملخصات تقييمات الصالات
    /// </summary>
    public DbSet<HallRatingSummary> HallRatingSummaries { get; set; }

    /// <summary>
    /// مجموعة التواريخ غير المتاحة للصالات
    /// </summary>
    public DbSet<HallUnavailableDate> HallUnavailableDates { get; set; }

    /// <summary>
    /// مجموعة طرق الدفع المتاحة للصالات
    /// </summary>
    public DbSet<HallPaymentMethod> HallPaymentMethods { get; set; }

    /// <summary>
    /// مجموعة الموظفين
    /// </summary>
    public DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// مجموعة الحجوزات
    /// </summary>
    public DbSet<Booking> Bookings { get; set; }

    /// <summary>
    /// مجموعة الفواتير
    /// </summary>
    public DbSet<Invoice> Invoices { get; set; }

    /// <summary>
    /// مجموعة بنود الفواتير
    /// </summary>
    public DbSet<InvoiceItems> InvoiceItems { get; set; }

    /// <summary>
    /// مجموعة وسائل الدفع
    /// </summary>
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    /// <summary>
    /// مجموعة المدفوعات
    /// </summary>
    public DbSet<Payment> Payments { get; set; }

    /// <summary>
    /// مجموعة تقييمات الخدمات
    /// </summary>
    public DbSet<ServiceRating> ServiceRatings { get; set; }

    /// <summary>
    /// مجموعة ملخصات تقييمات الخدمات
    /// </summary>
    public DbSet<ServiceRatingSummary> ServiceRatingSummaries { get; set; }

    /// <summary>
    /// مجموعة الإشعارات
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    #endregion

    /// <summary>
    /// تكوين نموذج قاعدة البيانات
    /// </summary>
    /// <param name="modelBuilder">بناء النموذج</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Customer - العميل

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.CustomerID);

            entity.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(c => c.Address)
                .HasMaxLength(300);

            entity.Property(c => c.ProfileImageUrl)
                .HasMaxLength(500);

            entity.Property(c => c.Gender)
                .HasMaxLength(20);

            entity.Property(c => c.Country)
                .HasMaxLength(100);

            entity.Property(c => c.City)
                .HasMaxLength(100);

            entity.Property(c => c.Nationality)
                .HasMaxLength(100);

            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع User
            entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة مع Bookings
            entity.HasMany(c => c.Bookings)
                .WithOne(b => b.Customer)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع HallRatings
            entity.HasMany(c => c.HallRatings)
                .WithOne(hr => hr.Customer)
                .HasForeignKey(hr => hr.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع ServiceRatings
            entity.HasMany(c => c.ServiceRatings)
                .WithOne(sr => sr.Customer)
                .HasForeignKey(sr => sr.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region HallOwner - صاحب الصالة

        modelBuilder.Entity<HallOwner>(entity =>
        {
            entity.HasKey(ho => ho.OwnerID);

            entity.Property(ho => ho.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(ho => ho.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(ho => ho.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(ho => ho.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(ho => ho.Address)
                .HasMaxLength(300);

            entity.Property(ho => ho.Gender)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(ho => ho.Nationality)
                .HasMaxLength(100);

            entity.Property(ho => ho.City)
                .HasMaxLength(100);

            entity.Property(ho => ho.Country)
                .HasMaxLength(100);

            entity.Property(ho => ho.ProfileImageUrl)
                .HasMaxLength(500);

            entity.Property(ho => ho.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع User
            entity.HasOne(ho => ho.User)
                .WithMany()
                .HasForeignKey(ho => ho.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة مع Halls
            entity.HasMany(ho => ho.Halls)
                .WithOne(h => h.Owner)
                .HasForeignKey(h => h.OwnerUserID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region HallCategorie - فئة الصالة

        modelBuilder.Entity<HallCategorie>(entity =>
        {
            entity.HasKey(hc => hc.CategoryID);

            entity.Property(hc => hc.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(hc => hc.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(hc => hc.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع Halls
            entity.HasMany(hc => hc.Halls)
                .WithOne(h => h.Category)
                .HasForeignKey(h => h.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region Hall - الصالة

        modelBuilder.Entity<Hall>(entity =>
        {
            entity.HasKey(h => h.HallID);

            entity.Property(h => h.HallName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(h => h.Description)
                .HasMaxLength(2000);

            entity.Property(h => h.Address)
                .HasMaxLength(300);

            entity.Property(h => h.PricingMode)
                .HasMaxLength(50);

            entity.Property(h => h.PricePerHour)
                .HasColumnType("decimal(18,2)");

            entity.Property(h => h.PricePerDay)
                .HasColumnType("decimal(18,2)");

            entity.Property(h => h.DefaultDepositAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(h => h.BaseCommissionRate)
                .HasColumnType("decimal(5,2)");

            entity.Property(h => h.MainImageUrl)
                .HasMaxLength(500);

            entity.Property(h => h.CancellationPolicy)
                .HasMaxLength(1000);

            entity.Property(h => h.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع HallRatingSummary (واحد لواحد)
            entity.HasOne(h => h.HallRatingSummary)
                .WithOne(hrs => hrs.Hall)
                .HasForeignKey<Hall>(h => h.HallRatingSummaryID)
                .OnDelete(DeleteBehavior.SetNull);

            // العلاقات المتعددة
            entity.HasMany(h => h.HallMedia)
                .WithOne(hm => hm.Hall)
                .HasForeignKey(hm => hm.HallID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(h => h.HallServices)
                .WithOne(hs => hs.Hall)
                .HasForeignKey(hs => hs.HallID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(h => h.Employees)
                .WithOne(e => e.Hall)
                .HasForeignKey(e => e.HallID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(h => h.HallRatings)
                .WithOne(hr => hr.Hall)
                .HasForeignKey(hr => hr.HallID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(h => h.Bookings)
                .WithOne(b => b.Hall)
                .HasForeignKey(b => b.HallId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(h => h.HallUnavailableDates)
                .WithOne(hud => hud.Hall)
                .HasForeignKey(hud => hud.HallID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(h => h.HallPaymentMethods)
                .WithOne(hpm => hpm.Hall)
                .HasForeignKey(hpm => hpm.HallID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        #endregion

        #region HallMedia - وسائط الصالة

        modelBuilder.Entity<HallMedia>(entity =>
        {
            entity.HasKey(hm => hm.MediaID);

            entity.Property(hm => hm.MediaTitle)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(hm => hm.MediaPath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(hm => hm.MediaType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(hm => hm.ThumbnailPath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(hm => hm.Caption)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(hm => hm.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region HallServices - خدمات الصالة

        modelBuilder.Entity<HallServices>(entity =>
        {
            entity.HasKey(hs => hs.ServiceId);

            entity.Property(hs => hs.ServiceName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(hs => hs.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(hs => hs.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(hs => hs.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع ServiceRatingSummary (واحد لواحد)
            entity.HasOne(hs => hs.ServiceRatingSummary)
                .WithOne(srs => srs.HallServices)
                .HasForeignKey<HallServices>(hs => hs.ServiceRatingSummaryID)
                .OnDelete(DeleteBehavior.SetNull);

            // العلاقات المتعددة
            entity.HasMany(hs => hs.InvoiceItems)
                .WithOne(ii => ii.HallService)
                .HasForeignKey(ii => ii.HallServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(hs => hs.ServiceRatings)
                .WithOne(sr => sr.HallServices)
                .HasForeignKey(sr => sr.HallServiceID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        #endregion

        #region HallRating - تقييم الصالة

        modelBuilder.Entity<HallRating>(entity =>
        {
            entity.HasKey(hr => hr.RatingID);

            entity.Property(hr => hr.Comment)
                .HasMaxLength(2000);

            entity.Property(hr => hr.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // إضافة قيد للتأكد من أن التقييم بين 1 و 5
            entity.HasCheckConstraint("CK_HallRating_OverallRating", "[OverallRating] >= 1 AND [OverallRating] <= 5");
        });

        #endregion

        #region HallRatingSummary - ملخص تقييم الصالة

        modelBuilder.Entity<HallRatingSummary>(entity =>
        {
            entity.HasKey(hrs => hrs.HallRatingSummaryId);

            entity.Property(hrs => hrs.OverallRatingAverage)
                .HasColumnType("decimal(3,2)");

            entity.Property(hrs => hrs.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region HallUnavailableDate - تاريخ غير متاح للصالة

        modelBuilder.Entity<HallUnavailableDate>(entity =>
        {
            entity.HasKey(hud => hud.UnavailableID);

            entity.Property(hud => hud.Reason)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(hud => hud.Notes)
                .HasMaxLength(1000);

            entity.Property(hud => hud.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region HallPaymentMethod - طريقة الدفع للصالة

        modelBuilder.Entity<HallPaymentMethod>(entity =>
        {
            entity.HasKey(hpm => hpm.HallPaymentMethodID);

            entity.Property(hpm => hpm.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع PaymentMethod
            entity.HasOne(hpm => hpm.PaymentMethod)
                .WithMany(pm => pm.HallPaymentMethods)
                .HasForeignKey(hpm => hpm.PaymentMethodID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region Employee - الموظف

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(e => e.Address)
                .HasMaxLength(300);

            entity.Property(e => e.ProfileImageUrl)
                .HasMaxLength(500);

            entity.Property(e => e.Gender)
                .HasMaxLength(20);

            entity.Property(e => e.Country)
                .HasMaxLength(100);

            entity.Property(e => e.City)
                .HasMaxLength(100);

            entity.Property(e => e.Nationality)
                .HasMaxLength(100);

            entity.Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region Booking - الحجز

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(b => b.BookingId);

            entity.Property(b => b.PricingMode)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(b => b.DepositAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(b => b.EventType)
                .HasMaxLength(100);

            entity.Property(b => b.Notes)
                .HasMaxLength(2000);

            entity.Property(b => b.DiscountPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع Invoices
            entity.HasMany(b => b.Invoices)
                .WithOne(i => i.Booking)
                .HasForeignKey(i => i.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region Invoice - الفاتورة

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.InvoiceId);

            entity.Property(i => i.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(i => i.PaidAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(i => i.BalanceDue)
                .HasColumnType("decimal(18,2)");

            entity.Property(i => i.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(i => i.Description)
                .HasMaxLength(1000);

            entity.Property(i => i.CommissionAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(i => i.CommissionPercentage)
                .HasColumnType("decimal(5,2)");

            entity.Property(i => i.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع InvoiceItems
            entity.HasMany(i => i.InvoiceItems)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة مع Payments
            entity.HasMany(i => i.Payments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // إضافة فهرس فريد لرقم الفاتورة
            entity.HasIndex(i => i.InvoiceNumber)
                .IsUnique();
        });

        #endregion

        #region InvoiceItems - بنود الفاتورة

        modelBuilder.Entity<InvoiceItems>(entity =>
        {
            entity.HasKey(ii => ii.ItemId);

            entity.Property(ii => ii.ItemName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(ii => ii.ItemType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(ii => ii.UnitType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(ii => ii.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(ii => ii.Total)
                .HasColumnType("decimal(18,2)");

            entity.Property(ii => ii.Note)
                .HasMaxLength(500);

            entity.Property(ii => ii.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region PaymentMethod - وسيلة الدفع

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(pm => pm.MethodId);

            entity.Property(pm => pm.MethodName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(pm => pm.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع Payments
            entity.HasMany(pm => pm.Payments)
                .WithOne(p => p.PaymentMethod)
                .HasForeignKey(p => p.MethodId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region Payment - الدفع

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);

            entity.Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Note)
                .HasMaxLength(500);

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region ServiceRating - تقييم الخدمة

        modelBuilder.Entity<ServiceRating>(entity =>
        {
            entity.HasKey(sr => sr.ServiceRatingID);

            entity.Property(sr => sr.Comment)
                .HasMaxLength(2000);

            entity.Property(sr => sr.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // إضافة قيد للتأكد من أن التقييم بين 1 و 5
            entity.HasCheckConstraint("CK_ServiceRating_Rating", "[Rating] >= 1 AND [Rating] <= 5");
        });

        #endregion

        #region ServiceRatingSummary - ملخص تقييم الخدمة

        modelBuilder.Entity<ServiceRatingSummary>(entity =>
        {
            entity.HasKey(srs => srs.ServiceRatingSummaryId);

            entity.Property(srs => srs.RatingAverage)
                .HasColumnType("decimal(3,2)");

            entity.Property(srs => srs.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        #endregion

        #region Notification - الإشعارات

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.NotificationID);

            entity.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(n => n.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(n => n.TargetScreen)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(n => n.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // العلاقة مع User
            entity.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        #endregion

        #region Identity Tables Customization - تخصيص جداول Identity

        // تخصيص أسماء جداول Identity
        modelBuilder.Entity<IdentityUser<Guid>>(entity =>
        {
            entity.ToTable("Users");
        });

        modelBuilder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable("Roles");
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        #endregion
    }

    /// <summary>
    /// حفظ التغييرات مع تحديث تاريخ التعديل تلقائياً
    /// </summary>
    /// <param name="cancellationToken">رمز الإلغاء</param>
    /// <returns>عدد الكيانات المتأثرة</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // تحديث UpdatedAt تلقائياً للكيانات المعدلة
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var updatedAtProperty = entry.Entity.GetType().GetProperty("UpdatedAt");
            if (updatedAtProperty != null && updatedAtProperty.PropertyType == typeof(DateTime?))
            {
                updatedAtProperty.SetValue(entry.Entity, DateTime.UtcNow);
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
