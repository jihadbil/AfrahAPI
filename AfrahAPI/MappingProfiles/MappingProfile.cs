using AutoMapper;
using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.PaymentMethod;
using AfrahAPI.Models.DTOs.HallCategorie;
using AfrahAPI.Models.DTOs.Customer;
using AfrahAPI.Models.DTOs.HallOwner;
using AfrahAPI.Models.DTOs.Employee;
using AfrahAPI.Models.DTOs.Hall;
using AfrahAPI.Models.DTOs.HallMedia;
using AfrahAPI.Models.DTOs.HallServices;
using AfrahAPI.Models.DTOs.HallPaymentMethod;
using AfrahAPI.Models.DTOs.HallUnavailableDate;
using AfrahAPI.Models.DTOs.Booking;
using AfrahAPI.Models.DTOs.Invoice;
using AfrahAPI.Models.DTOs.InvoiceItems;
using AfrahAPI.Models.DTOs.Payment;
using AfrahAPI.Models.DTOs.HallRating;
using AfrahAPI.Models.DTOs.HallRatingSummary;
using AfrahAPI.Models.DTOs.ServiceRating;
using AfrahAPI.Models.DTOs.ServiceRatingSummary;
using AfrahAPI.Models.DTOs.Notification;

namespace AfrahAPI.MappingProfiles;

/// <summary>
/// ملف تعريف AutoMapper الشامل لجميع الموديلات والـ DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ==================== PaymentMethod Mappings ====================
        CreateMap<PaymentMethod, PaymentMethodReadDTO>();
        CreateMap<PaymentMethodCreateDTO, PaymentMethod>();
        CreateMap<PaymentMethodUpdateDTO, PaymentMethod>();

        // ==================== HallCategorie Mappings ====================
        CreateMap<HallCategorie, HallCategorieReadDTO>();
        CreateMap<HallCategorieCreateDTO, HallCategorie>();
        CreateMap<HallCategorieUpdateDTO, HallCategorie>();

        // ==================== Customer Mappings ====================
        CreateMap<Customer, CustomerReadDTO>();
        CreateMap<CustomerCreateDTO, Customer>();
        CreateMap<CustomerUpdateDTO, Customer>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallOwner Mappings ====================
        CreateMap<HallOwner, HallOwnerReadDTO>();
        CreateMap<HallOwnerCreateDTO, HallOwner>();
        CreateMap<HallOwnerUpdateDTO, HallOwner>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== Employee Mappings ====================
        CreateMap<Employee, EmployeeReadDTO>();
        CreateMap<EmployeeCreateDTO, Employee>();
        CreateMap<EmployeeUpdateDTO, Employee>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== Hall Mappings ====================
        CreateMap<Hall, HallReadDTO>();
        CreateMap<Hall, HallListDTO>();
        CreateMap<HallCreateDTO, Hall>();
        CreateMap<HallUpdateDTO, Hall>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallMedia Mappings ====================
        CreateMap<HallMedia, HallMediaReadDTO>();
        CreateMap<HallMediaCreateDTO, HallMedia>();
        CreateMap<HallMediaUpdateDTO, HallMedia>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallServices Mappings ====================
        CreateMap<HallServices, HallServicesReadDTO>();
        CreateMap<HallServicesCreateDTO, HallServices>();
        CreateMap<HallServicesUpdateDTO, HallServices>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallPaymentMethod Mappings ====================
        CreateMap<HallPaymentMethod, HallPaymentMethodReadDTO>();
        CreateMap<HallPaymentMethodCreateDTO, HallPaymentMethod>();
        CreateMap<HallPaymentMethodUpdateDTO, HallPaymentMethod>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallUnavailableDate Mappings ====================
        CreateMap<HallUnavailableDate, HallUnavailableDateReadDTO>();
        CreateMap<HallUnavailableDateCreateDTO, HallUnavailableDate>();
        CreateMap<HallUnavailableDateUpdateDTO, HallUnavailableDate>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== Booking Mappings ====================
        CreateMap<Booking, BookingReadDTO>();
        CreateMap<BookingCreateDTO, Booking>();
        CreateMap<BookingUpdateDTO, Booking>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== Invoice Mappings ====================
        CreateMap<Invoice, InvoiceReadDTO>();
        CreateMap<InvoiceCreateDTO, Invoice>();
        CreateMap<InvoiceUpdateDTO, Invoice>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== InvoiceItems Mappings ====================
        CreateMap<InvoiceItems, InvoiceItemsReadDTO>();
        CreateMap<InvoiceItemsCreateDTO, InvoiceItems>();
        CreateMap<InvoiceItemsUpdateDTO, InvoiceItems>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== Payment Mappings ====================
        CreateMap<Payment, PaymentReadDTO>();
        CreateMap<PaymentCreateDTO, Payment>();
        CreateMap<PaymentUpdateDTO, Payment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallRating Mappings ====================
        CreateMap<HallRating, HallRatingReadDTO>();
        CreateMap<HallRatingCreateDTO, HallRating>();
        CreateMap<HallRatingUpdateDTO, HallRating>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== HallRatingSummary Mappings ====================
        CreateMap<HallRatingSummary, HallRatingSummaryReadDTO>();

        // ==================== ServiceRating Mappings ====================
        CreateMap<ServiceRating, ServiceRatingReadDTO>();
        CreateMap<ServiceRatingCreateDTO, ServiceRating>();
        CreateMap<ServiceRatingUpdateDTO, ServiceRating>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ==================== ServiceRatingSummary Mappings ====================
        CreateMap<ServiceRatingSummary, ServiceRatingSummaryReadDTO>();

        // ==================== Notification Mappings ====================
        CreateMap<Notification, NotificationReadDTO>();
        CreateMap<NotificationCreateDTO, Notification>();
        CreateMap<NotificationUpdateDTO, Notification>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
