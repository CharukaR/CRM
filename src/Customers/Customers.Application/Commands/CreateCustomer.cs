using MediatR;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;

namespace Customers.Application.Commands;

// Command to create a new customer with the specified name, email, and phone
public record CreateCustomerCommand(string Name, string Email, string Phone) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    // Constructor to initialize the repository, ensuring it's not null
    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    // Handles the creation of a new customer
    public async Task<CustomerDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine("Handling CreateCustomerCommand...");

        // Create a new customer entity from the command data
        var customer = Customer.Create(command.Name, command.Email, command.Phone);
        Console.WriteLine($"Customer created with Name: {customer.Name}, Email: {customer.Email}, Phone: {customer.Phone}");

        try
        {
            // Attempt to add the new customer to the repository
            await _repository.AddAsync(customer, cancellationToken);
            Console.WriteLine("Customer added to repository successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception before rethrowing
            Console.WriteLine($"Exception occurred while adding customer: {ex.Message}");
            throw; // Rethrow the original exception
        }

        // Return a DTO representing the newly created customer
        var customerDto = new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );
        Console.WriteLine("Returning CustomerDto with customer details.");

        return customerDto;
    }
}