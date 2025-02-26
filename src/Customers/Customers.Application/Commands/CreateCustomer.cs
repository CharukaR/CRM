using MediatR;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Customers.Application.Commands;

// Command to create a new customer with name, email, and phone
public record CreateCustomerCommand(string n, string e, string p) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    // Constructor with dependency injection for repository and logger
    public CreateCustomerCommandHandler(ICustomerRepository repo, ILogger<CreateCustomerCommandHandler> logger)
    {
        _repository = repo ?? throw new ArgumentNullException(nameof(repo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Handle method to process the CreateCustomerCommand
    public async Task<CustomerDto> Handle(CreateCustomerCommand cmd, CancellationToken tkn)
    {
        _logger.LogInformation("Handling CreateCustomerCommand for Name: {Name}, Email: {Email}", cmd.n, cmd.e);

        var name = cmd.n;
        var email = cmd.e;
        var phone = cmd.p;

        // Create a new customer entity
        var customer = Customer.Create(name, email, phone);
        _logger.LogDebug("Customer entity created with Name: {Name}, Email: {Email}", customer.Name, customer.Email);

        try
        {
            // Add the new customer to the repository
            await _repository.AddAsync(customer, tkn);
            _logger.LogInformation("Customer added to repository with ID: {CustomerId}", customer.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding customer to repository");
            throw; // Rethrow the original exception
        }

        // Return a DTO representation of the customer
        var customerDto = new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );

        _logger.LogInformation("Returning CustomerDto with ID: {CustomerId}", customerDto.Id);
        return customerDto;
    }
}