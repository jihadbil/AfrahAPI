namespace AfrahApp.Models.Customer;

public sealed class CustomerReadResponse
{
    public Guid CustomerID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Guid UserID { get; set; }
}
