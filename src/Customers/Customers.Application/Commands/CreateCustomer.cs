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

        // Extracting command parameters
        var name = cmd.n;
        var email = cmd.e;
        var phone = cmd.p;

        Console.WriteLine($"Creating customer with Name: {name}, Email: {email}, Phone: {phone}");

        // Create a new customer entity
        var customer = Customer.Create(name, email, phone);

        try
        {
            // Attempt to add the new customer to the repository
            Console.WriteLine("Adding customer to repository...");
            await _repository.AddAsync(customer, tkn);
            Console.WriteLine("Customer added successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception and rethrow it
            Console.WriteLine($"Exception occurred while adding customer: {ex.Message}");
            throw ex;
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

        Console.WriteLine("Returning CustomerDto...");
        return customerDto;
    }
}