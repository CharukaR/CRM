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
}