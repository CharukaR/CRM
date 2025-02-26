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
        _logger.LogInformation("Starting GetByIdAsync method with id: {Id}", id);
        
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
        
        if (customer == null)
        {
            _logger.LogWarning("Customer with id: {Id} not found", id);
        }
        else
        {
            _logger.LogInformation("Customer with id: {Id} retrieved successfully", id);
        }
        
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting GetAllAsync method");
        
        var customers = await _context.Customers.ToListAsync(cancellationToken);
        
        _logger.LogInformation("Retrieved {Count} customers", customers.Count);
        
        return customers;
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting AddAsync method for customer: {Customer}", customer);
        
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Customer added successfully with id: {Id}", customer.Id);
        
        return customer;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting UpdateAsync method for customer: {Customer}", customer);
        
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Customer with id: {Id} updated successfully", customer.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting DeleteAsync method with id: {Id}", id);
        
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer == null)
        {
            _logger.LogWarning("Customer with id: {Id} not found, nothing to delete", id);
            return;
        }
        
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Customer with id: {Id} deleted successfully", id);
    }
}