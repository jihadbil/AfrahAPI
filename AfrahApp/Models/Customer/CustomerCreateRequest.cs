namespace AfrahApp.Models.Customer;

public sealed class CustomerCreateRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Gender { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Nationality { get; set; }
    public Guid UserID { get; set; }
}
