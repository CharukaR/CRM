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
        _logger.LogInformation("Handling CreateCustomerCommand for Name: {Name}, Email: {Email}, Phone: {Phone}", command.Name, command.Email, command.Phone);

        // Create a new customer entity
        var customer = Customer.Create(command.Name, command.Email, command.Phone);
        _logger.LogDebug("Customer entity created with Name: {Name}, Email: {Email}, Phone: {Phone}", customer.Name, customer.Email, customer.Phone);

        try
        {
            // Attempt to add the new customer to the repository
            await _repository.AddAsync(customer, cancellationToken);
            _logger.LogInformation("Customer successfully added to the repository with ID: {Id}", customer.Id);
        }
        catch (Exception ex)
        {
            // Log the exception before rethrowing
            _logger.LogError(ex, "Error occurred while adding customer to the repository");
            throw; // Rethrow the original exception
        }

        // Return a DTO representation of the newly created customer
        var customerDto = new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );

        _logger.LogInformation("Returning CustomerDto with ID: {Id}", customerDto.Id);
        return customerDto;
    }
}