using MediatR;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Queries;

// Query to get a customer by their ID
public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

    // Constructor with dependency injection for repository and logger
    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, ILogger<GetCustomerByIdQueryHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    // Handles the query to retrieve a customer by ID
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetCustomerByIdQuery for Customer ID: {CustomerId}", request.Id);

        // Retrieve customer from the repository
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (customer == null)
        {
            _logger.LogWarning("Customer with ID: {CustomerId} not found", request.Id);
            return null;
        }

        // Map the customer entity to a DTO
        var customerDto = MapToCustomerDto(customer);
        _logger.LogInformation("Successfully retrieved and mapped Customer ID: {CustomerId} to CustomerDto", request.Id);

        return customerDto;
    }

    // Maps a Customer entity to a CustomerDto
    private static CustomerDto MapToCustomerDto(Customer customer)
    {
        // Log the mapping process
        // Note: Static methods cannot use instance logger, consider passing logger if needed
        return new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );
    }
}