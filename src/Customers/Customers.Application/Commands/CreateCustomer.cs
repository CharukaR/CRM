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
        var customer = Customer.Create(cmd.n, cmd.e, cmd.p);

        try
        {
            await _repository.AddAsync(customer, tkn);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while adding customer: {ex.Message}");
            throw;
        }

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