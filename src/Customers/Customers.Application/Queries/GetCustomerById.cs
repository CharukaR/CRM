using MediatR;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Queries;

// Query to get a customer by their unique identifier
public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

    // Constructor to inject dependencies
    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, ILogger<GetCustomerByIdQueryHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    // Handles the query to retrieve a customer by ID
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetCustomerByIdQuery for Customer ID: {CustomerId}", request.Id);

        // Attempt to retrieve the customer from the repository
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (customer == null)
        {
            _logger.LogWarning("Customer with ID: {CustomerId} not found", request.Id);
            return null;
        }

        // Log the successful retrieval of the customer
        _logger.LogInformation("Customer with ID: {CustomerId} retrieved successfully", request.Id);

        // Map the customer entity to a CustomerDto
        var customerDto = new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );

        _logger.LogInformation("Returning CustomerDto for Customer ID: {CustomerId}", request.Id);
        return customerDto;
    }
}