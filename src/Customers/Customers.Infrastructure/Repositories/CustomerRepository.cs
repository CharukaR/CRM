using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Data;

namespace Customers.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomersDbContext _context;

    public CustomerRepository(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"GetByIdAsync: Attempting to retrieve customer with ID: {id}");
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
        Console.WriteLine(customer != null ? $"GetByIdAsync: Customer with ID: {id} found." : $"GetByIdAsync: Customer with ID: {id} not found.");
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetAllAsync: Retrieving all customers.");
        var customers = await _context.Customers.ToListAsync(cancellationToken);
        Console.WriteLine($"GetAllAsync: Retrieved {customers.Count} customers.");
        return customers;
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"AddAsync: Adding a new customer with ID: {customer.Id}");
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"AddAsync: Customer with ID: {customer.Id} added successfully.");
        return customer;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"UpdateAsync: Updating customer with ID: {customer.Id}");
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"UpdateAsync: Customer with ID: {customer.Id} updated successfully.");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DeleteAsync: Attempting to delete customer with ID: {id}");
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer == null)
        {
            Console.WriteLine($"DeleteAsync: Customer with ID: {id} not found. No deletion performed.");
            return;
        }
        
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"DeleteAsync: Customer with ID: {id} deleted successfully.");
    }
}