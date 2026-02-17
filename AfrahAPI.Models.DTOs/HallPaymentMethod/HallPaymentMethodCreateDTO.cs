using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallPaymentMethod;

public class HallPaymentMethodCreateDTO
{
    [Required(ErrorMessage = "معرف الصالة مطلوب")]
    public Guid HallID { get; set; }

    [Required(ErrorMessage = "معرف وسيلة الدفع مطلوب")]
    public Guid PaymentMethodID { get; set; }

    public bool IsActive { get; set; } = true;
}
