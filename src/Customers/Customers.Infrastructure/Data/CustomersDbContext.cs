using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;

namespace Customers.Infrastructure.Data;

public class CustomersDbContext : DbContext
{
    // Constructor for CustomersDbContext, initializing with DbContextOptions
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options) 
    {
        // Log the initialization of the DbContext
        Console.WriteLine("CustomersDbContext initialized with options.");
    }

    // DbSet representing the Customers table in the database
    public DbSet<Customer> Customers { get; set; }

    // Override the OnModelCreating method to configure entity mappings
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Log the start of the model creation process
        Console.WriteLine("OnModelCreating started.");

        // Configure the Customer entity
        ConfigureCustomerEntity(modelBuilder);

        // Log the completion of the model creation process
        Console.WriteLine("OnModelCreating completed.");
    }

    // Method to configure the Customer entity
    private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
    {
        // Log the start of the Customer entity configuration
        Console.WriteLine("Configuring Customer entity.");

        modelBuilder.Entity<Customer>(entity =>
        {
            // Set the primary key for the Customer entity
            entity.HasKey(e => e.Id);
            // Configure the Name property as required with a maximum length of 200
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            // Configure the Email property as required with a maximum length of 200
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            // Configure the Phone property with a maximum length of 20
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        // Log the completion of the Customer entity configuration
        Console.WriteLine("Customer entity configuration completed.");
    }
}