namespace Customers.Application.DTOs;

/// <summary>
/// Data Transfer Object for Customer information.
/// </summary>
/// <param name="Id">Unique identifier for the customer.</param>
/// <param name="Name">Name of the customer.</param>
/// <param name="Email">Email address of the customer.</param>
/// <param name="Phone">Phone number of the customer.</param>
/// <param name="CreatedAt">Timestamp when the customer was created.</param>
/// <param name="UpdatedAt">Timestamp when the customer was last updated. Nullable if never updated.</param>
/// <param name="IsActive">Indicates whether the customer is currently active.</param>
public record CustomerDto(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
);