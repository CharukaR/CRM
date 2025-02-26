using Customers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Customers.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        // Retrieves a customer by their unique identifier.
        // Logs the start and end of the method, and the ID being searched for.
        Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        // Retrieves all customers.
        // Logs the start and end of the method.
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

        // Adds a new customer to the repository.
        // Logs the start and end of the method, and details of the customer being added.
        Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);

        // Updates an existing customer's information.
        // Logs the start and end of the method, and details of the customer being updated.
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

        // Deletes a customer by their unique identifier.
        // Logs the start and end of the method, and the ID of the customer being deleted.
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}