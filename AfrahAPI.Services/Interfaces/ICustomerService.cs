using AfrahAPI.Models;
using AfrahAPI.Models.DTOs.Customer;

namespace AfrahAPI.Services.Interfaces;

/// <summary>
/// واجهة خدمة إدارة العملاء
/// </summary>
public interface ICustomerService : IBaseService<Customer, CustomerCreateDTO, CustomerReadDTO, CustomerUpdateDTO>
{
    /// <summary>
    /// الحصول على عميل بواسطة معرف المستخدم المرتبط
    /// </summary>
    /// <param name="userId">معرف المستخدم</param>
    /// <returns>DTO القراءة للعميل أو null</returns>
    Task<CustomerReadDTO?> GetCustomerByUserIdAsync(Guid userId);

    /// <summary>
    /// الحصول على جميع حجوزات العميل
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <returns>قائمة بحجوزات العميل</returns>
    Task<IEnumerable<object>> GetCustomerBookingsAsync(Guid customerId);

    /// <summary>
    /// تحديث ملف العميل الشخصي
    /// </summary>
    /// <param name="customerId">معرف العميل</param>
    /// <param name="updateDto">بيانات التحديث</param>
    /// <returns>DTO القراءة المُحدث</returns>
    Task<CustomerReadDTO?> UpdateProfileAsync(Guid customerId, CustomerUpdateDTO updateDto);
}
