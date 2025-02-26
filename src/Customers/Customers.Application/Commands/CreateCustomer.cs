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
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    // Constructor to initialize the repository and logger
    public CreateCustomerCommandHandler(ICustomerRepository repository, ILogger<CreateCustomerCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Handles the creation of a new customer
    public async Task<CustomerDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateCustomerCommand for Name: {Name}, Email: {Email}", command.Name, command.Email);

        // Create a new customer entity
        var customer = Customer.Create(command.Name, command.Email, command.Phone);
        _logger.LogDebug("Customer entity created with Name: {Name}, Email: {Email}", customer.Name, customer.Email);

        try
        {
            // Attempt to add the new customer to the repository
            await _repository.AddAsync(customer, cancellationToken);
            _logger.LogInformation("Customer added successfully with ID: {CustomerId}", customer.Id);
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow it
            _logger.LogError(ex, "Error adding customer with Name: {Name}, Email: {Email}", command.Name, command.Email);
            throw new Exception("Error adding customer", ex);
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