using MediatR;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;

namespace Customers.Application.Commands;

// Command to create a new customer with name, email, and phone
public record CreateCustomerCommand(string n, string e, string p) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    // Constructor to inject the customer repository dependency
    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    // Handles the creation of a new customer
    public async Task<CustomerDto> Handle(CreateCustomerCommand cmd, CancellationToken tkn)
    {
        Console.WriteLine("Handling CreateCustomerCommand...");

        // Create a new customer entity using the provided command data
        var customer = Customer.Create(cmd.n, cmd.e, cmd.p);
        Console.WriteLine($"Customer created with Name: {customer.Name}, Email: {customer.Email}, Phone: {customer.Phone}");

        try
        {
            // Attempt to add the new customer to the repository
            await _repository.AddAsync(customer, tkn);
            Console.WriteLine("Customer successfully added to the repository.");
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during the add operation
            Console.WriteLine($"Exception occurred while adding customer: {ex.Message}");
            throw;
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

        Console.WriteLine("Returning CustomerDto...");
        return customerDto;
    }
}