using System;

namespace AfrahAPI.Models.DTOs.HallPaymentMethod;

public class HallPaymentMethodReadDTO
{
    public Guid HallPaymentMethodID { get; set; }
    public Guid HallID { get; set; }
    public Guid PaymentMethodID { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
