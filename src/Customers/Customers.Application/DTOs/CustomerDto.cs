namespace Customers.Application.DTOs
{
    // Record type for transferring customer data
    public record CustomerDto(
        Guid Id,          // Unique identifier for the customer
        string Name,      // Customer's name
        string Email,     // Customer's email address
        string Phone,     // Customer's phone number
        DateTime CreatedAt, // Timestamp when the customer was created
        DateTime? UpdatedAt, // Timestamp when the customer was last updated, nullable
        bool IsActive     // Status indicating if the customer is active
    );
    
    // Constructor for CustomerDto
    public CustomerDto(Guid id, string name, string email, string phone, DateTime createdAt, DateTime? updatedAt, bool isActive)
    {
        Console.WriteLine("Initializing CustomerDto with Id: " + id);
        
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        IsActive = isActive;

        Console.WriteLine("CustomerDto initialized: " + this.ToString());
    }
}