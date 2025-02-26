using MediatR;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;

namespace Customers.Application.Queries;

// Query to get a customer by their unique identifier
public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;

// Handler for processing the GetCustomerByIdQuery
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;

    // Constructor to inject the customer repository dependency
    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    // Method to handle the query and return a CustomerDto if found
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling GetCustomerByIdQuery for Customer ID: {request.Id}");

        // Retrieve the customer from the repository using the provided ID
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        // Log the result of the repository call
        if (customer == null)
        {
            Console.WriteLine($"Customer with ID {request.Id} not found.");
            return null;
        }

        Console.WriteLine($"Customer with ID {request.Id} found. Preparing CustomerDto.");

        // Map the customer entity to a CustomerDto and return it
        var customerDto = new CustomerDto(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.CreatedAt,
            customer.UpdatedAt,
            customer.IsActive
        );

        Console.WriteLine($"CustomerDto for Customer ID {request.Id} created successfully.");
        return customerDto;
    }
}