using MediatR;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Application.DTOs;

namespace Customers.Application.Commands;

public record CreateCustomerCommand(string n, string e, string p) : IRequest<CustomerDto>;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repo)
    {
        _repository = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand cmd, CancellationToken tkn)
    {
        var name = cmd.n;
        var email = cmd.e;
        var phone = cmd.p;

        var customer = Customer.Create(name, email, phone);

        try
        {
            await _repository.AddAsync(customer, tkn);
        }
        catch (Exception ex)
        {
            throw; // Rethrow the original exception
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