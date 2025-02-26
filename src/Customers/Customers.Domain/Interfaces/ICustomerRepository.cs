using Customers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Customers.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Retrieves a customer by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The customer entity if found; otherwise, null.</returns>
        Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A collection of all customer entities.</returns>
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer entity to add.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The added customer entity.</returns>
        Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing customer in the repository.
        /// </summary>
        /// <param name="customer">The customer entity with updated information.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a customer from the repository by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}