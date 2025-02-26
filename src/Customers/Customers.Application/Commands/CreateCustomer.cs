using MediatR;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;

namespace Customers.Application.Commands;

public record CreateCustomerCommand(string Name, string Email, string Phone) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(command.Name, command.Email, command.Phone);

        try
        {
            await _repository.AddAsync(customer, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding customer", ex);
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