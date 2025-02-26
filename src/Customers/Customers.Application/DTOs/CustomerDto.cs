namespace Customers.Application.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
); 