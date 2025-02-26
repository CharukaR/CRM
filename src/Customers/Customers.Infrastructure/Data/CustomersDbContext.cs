using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;

namespace Customers.Infrastructure.Data
{
    public class CustomersDbContext : DbContext
    {
        // Constructor to initialize the DbContext with the provided options
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
        {
            Console.WriteLine("Initializing CustomersDbContext with provided options.");
        }

        // DbSet representing the Customers table in the database
        public DbSet<Customer> Customers { get; set; }

        // Method to configure the model and its entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("Starting OnModelCreating to configure the model.");

            modelBuilder.Entity<Customer>(entity =>
            {
                Console.WriteLine("Configuring Customer entity.");

                // Setting the primary key for the Customer entity
                entity.HasKey(e => e.Id);

                // Configuring the Name property to be required with a maximum length of 200 characters
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

                // Configuring the Email property to be required with a maximum length of 200 characters
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);

                // Configuring the Phone property with a maximum length of 20 characters
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            Console.WriteLine("Completed OnModelCreating.");
        }
    }
}