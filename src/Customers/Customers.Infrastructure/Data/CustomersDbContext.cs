using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;

namespace Customers.Infrastructure.Data;

public class CustomersDbContext : DbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
    {
        // Log the initialization of the CustomersDbContext with the provided options
        Console.WriteLine("Initializing CustomersDbContext with provided options.");
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Log the start of the OnModelCreating method
        Console.WriteLine("OnModelCreating method started.");

        modelBuilder.Entity<Customer>(entity =>
        {
            // Log the configuration of the Customer entity
            Console.WriteLine("Configuring Customer entity.");

            entity.HasKey(e => e.Id);
            // Log the setting of the primary key
            Console.WriteLine("Set primary key for Customer entity.");

            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            // Log the configuration of the Name property
            Console.WriteLine("Configured Name property: Required, MaxLength 200.");

            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            // Log the configuration of the Email property
            Console.WriteLine("Configured Email property: Required, MaxLength 200.");

            entity.Property(e => e.Phone).HasMaxLength(20);
            // Log the configuration of the Phone property
            Console.WriteLine("Configured Phone property: MaxLength 20.");
        });

        // Log the completion of the OnModelCreating method
        Console.WriteLine("OnModelCreating method completed.");
    }
}