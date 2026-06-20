using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using AfrahAPI.DataAccess;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// إضافة DbContext مع SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// إضافة Identity مع الإعدادات المخصصة
builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// إضافة Repository Pattern و Unit of Work
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IUnitOfWork, AfrahAPI.DataAccess.Repositories.Implementations.UnitOfWork>();

// تسجيل جميع الـ Repositories
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.ICustomerRepository, AfrahAPI.DataAccess.Repositories.Implementations.CustomerRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallOwnerRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallOwnerRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallCategorieRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallCategorieRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallMediaRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallMediaRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallServicesRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallServicesRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallRatingRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallRatingRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallRatingSummaryRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallRatingSummaryRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallUnavailableDateRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallUnavailableDateRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IHallPaymentMethodRepository, AfrahAPI.DataAccess.Repositories.Implementations.HallPaymentMethodRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IEmployeeRepository, AfrahAPI.DataAccess.Repositories.Implementations.EmployeeRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IBookingRepository, AfrahAPI.DataAccess.Repositories.Implementations.BookingRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IInvoiceRepository, AfrahAPI.DataAccess.Repositories.Implementations.InvoiceRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IInvoiceItemsRepository, AfrahAPI.DataAccess.Repositories.Implementations.InvoiceItemsRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IPaymentMethodRepository, AfrahAPI.DataAccess.Repositories.Implementations.PaymentMethodRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IPaymentRepository, AfrahAPI.DataAccess.Repositories.Implementations.PaymentRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IServiceRatingRepository, AfrahAPI.DataAccess.Repositories.Implementations.ServiceRatingRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.IServiceRatingSummaryRepository, AfrahAPI.DataAccess.Repositories.Implementations.ServiceRatingSummaryRepository>();
builder.Services.AddScoped<AfrahAPI.DataAccess.Repositories.Interfaces.INotificationRepository, AfrahAPI.DataAccess.Repositories.Implementations.NotificationRepository>();

// تسجيل Data Seeder
builder.Services.AddScoped<AfrahAPI.DataAccess.Seeders.DbSeeder>();

// تسجيل AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// تسجيل جميع الخدمات (Services)
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.ICustomerService, AfrahAPI.Services.Implementations.CustomerService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallOwnerService, AfrahAPI.Services.Implementations.HallOwnerService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallService, AfrahAPI.Services.Implementations.HallService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallCategorieService, AfrahAPI.Services.Implementations.HallCategorieService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallMediaService, AfrahAPI.Services.Implementations.HallMediaService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallServicesService, AfrahAPI.Services.Implementations.HallServicesService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallUnavailableDateService, AfrahAPI.Services.Implementations.HallUnavailableDateService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallPaymentMethodService, AfrahAPI.Services.Implementations.HallPaymentMethodService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IBookingService, AfrahAPI.Services.Implementations.BookingService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IPaymentService, AfrahAPI.Services.Implementations.PaymentService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IPaymentMethodService, AfrahAPI.Services.Implementations.PaymentMethodService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IInvoiceService, AfrahAPI.Services.Implementations.InvoiceService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IInvoiceItemsService, AfrahAPI.Services.Implementations.InvoiceItemsService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IHallRatingService, AfrahAPI.Services.Implementations.HallRatingService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IServiceRatingService, AfrahAPI.Services.Implementations.ServiceRatingService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.INotificationService, AfrahAPI.Services.Implementations.NotificationService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IAuthService, AfrahAPI.Services.Implementations.AuthService>();
builder.Services.AddScoped<AfrahAPI.Services.Interfaces.IEmployeeService, AfrahAPI.Services.Implementations.EmployeeService>();
// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});


// CORS Configuration - السماح لأي مصدر
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var seeder = services.GetRequiredService<AfrahAPI.DataAccess.Seeders.DbSeeder>();
        await seeder.SeedAllAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // إضافة واجهة Scalar لعرض OpenAPI
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
