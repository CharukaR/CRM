using Customers.Domain.Entities;
namespace Customers.Domain.Interfaces;

public interface ICustomerRepository
{
    // Retrieves a customer by their unique identifier.
    // Logs the start and end of the method execution.
    Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    // Retrieves all customers.
    // Logs the start and end of the method execution.
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    // Adds a new customer to the repository.
    // Logs the start and end of the method execution, including the customer details being added.
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);

    // Updates an existing customer's information.
    // Logs the start and end of the method execution, including the customer details being updated.
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

    // Deletes a customer by their unique identifier.
    // Logs the start and end of the method execution, including the ID of the customer being deleted.
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}