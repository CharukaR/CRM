using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Customers.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(CustomersDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting GetByIdAsync with id: {Id}", id);
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
        _logger.LogInformation("Completed GetByIdAsync with id: {Id}. Customer found: {CustomerFound}", id, customer != null);
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting GetAllAsync");
        var customers = await _context.Customers.ToListAsync(cancellationToken);
        _logger.LogInformation("Completed GetAllAsync. Number of customers retrieved: {Count}", customers.Count);
        return customers;
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting AddAsync for customer: {CustomerName}", customer.Name);
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Completed AddAsync for customer: {CustomerName}", customer.Name);
        return customer;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting UpdateAsync for customer: {CustomerId}", customer.Id);
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Completed UpdateAsync for customer: {CustomerId}", customer.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting DeleteAsync for customer id: {Id}", id);
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer == null)
        {
            _logger.LogWarning("DeleteAsync: Customer with id {Id} not found", id);
            return;
        }
        
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Completed DeleteAsync for customer id: {Id}", id);
    }
}