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
        Console.WriteLine($"GetByIdAsync started with id: {id}");
        
        // Attempt to find the customer by ID
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
        
        Console.WriteLine(customer != null ? $"Customer found: {customer.Id}" : "Customer not found");
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("GetAllAsync started");
        
        // Retrieve all customers from the database
        var customers = await _context.Customers.ToListAsync(cancellationToken);
        
        Console.WriteLine($"Total customers retrieved: {customers.Count}");
        return customers;
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"AddAsync started for customer: {customer.Id}");
        
        // Add the new customer to the database
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        Console.WriteLine($"Customer added with id: {customer.Id}");
        return customer;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"UpdateAsync started for customer: {customer.Id}");
        
        // Update the existing customer details
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        
        Console.WriteLine($"Customer updated with id: {customer.Id}");
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"DeleteAsync started for id: {id}");
        
        // Find the customer by ID and remove if exists
        var customer = await GetByIdAsync(id, cancellationToken);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"Customer deleted with id: {id}");
        }
        else
        {
            Console.WriteLine("Customer not found, nothing to delete");
        }
    }
}