using System;
using System.ComponentModel.DataAnnotations;

namespace AfrahAPI.Models.DTOs.HallPaymentMethod;

public class HallPaymentMethodUpdateDTO
{
    [Required(ErrorMessage = "معرف طريقة الدفع للصالة مطلوب")]
    public Guid HallPaymentMethodID { get; set; }

    public bool IsActive { get; set; }
}
