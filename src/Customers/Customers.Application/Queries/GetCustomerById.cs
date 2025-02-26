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
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (customer == null)
        {
            return null;
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