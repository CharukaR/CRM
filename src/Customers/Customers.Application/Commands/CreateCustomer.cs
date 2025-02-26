using MediatR;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Commands;

// Command to create a new customer with the specified details
public record CreateCustomerCommand(string Name, string Email, string Phone) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    // Constructor with dependency injection for the customer repository and logger
    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, ILogger<CreateCustomerCommandHandler> logger)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Handles the creation of a new customer
    public async Task<CustomerDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateCustomerCommand for Name: {Name}, Email: {Email}, Phone: {Phone}", command.Name, command.Email, command.Phone);

        // Create a new customer entity
        var customer = Customer.Create(command.Name, command.Email, command.Phone);
        _logger.LogDebug("Customer entity created with Name: {Name}, Email: {Email}, Phone: {Phone}", customer.Name, customer.Email, customer.Phone);

        try
        {
            // Attempt to add the new customer to the repository
            await _customerRepository.AddAsync(customer, cancellationToken);
            _logger.LogInformation("Customer added successfully with ID: {CustomerId}", customer.Id);
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow it with additional context
            _logger.LogError(ex, "An error occurred while adding the customer with Name: {Name}, Email: {Email}", command.Name, command.Email);
            throw new Exception("An error occurred while adding the customer.", ex);
        }

        // Return the customer data transfer object
        var customerDto = new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );

        _logger.LogInformation("Returning CustomerDto for Customer ID: {CustomerId}", customerDto.Id);
        return customerDto;
    }
}