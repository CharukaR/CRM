namespace Customers.Application.DTOs;

/// <summary>
/// Data Transfer Object for Customer information.
/// </summary>
public record CustomerDto(
    Guid Id, // Unique identifier for the customer
    string Name, // Name of the customer
    string Email, // Email address of the customer
    string Phone, // Phone number of the customer
    DateTime CreatedAt, // Timestamp when the customer was created
    DateTime? UpdatedAt, // Timestamp when the customer was last updated, nullable
    bool IsActive // Status indicating if the customer is active
);