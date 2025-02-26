using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Customers.Infrastructure.Data
{
    public class CustomersDbContext : DbContext
    {
        private readonly ILogger<CustomersDbContext> _logger;

        // Constructor with dependency injection for DbContextOptions and ILogger
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options, ILogger<CustomersDbContext> logger) : base(options)
        {
            _logger = logger;
            _logger.LogInformation("CustomersDbContext initialized.");
        }

        // DbSet representing the Customers table in the database
        public DbSet<Customer> Customers { get; set; }

        // Override method to configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("OnModelCreating started.");
            ConfigureCustomerEntity(modelBuilder);
            _logger.LogInformation("OnModelCreating completed.");
        }

        // Method to configure the Customer entity
        private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("Configuring Customer entity.");
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id); // Setting Id as the primary key
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200); // Name is required with max length 200
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200); // Email is required with max length 200
                entity.Property(e => e.Phone).HasMaxLength(20); // Phone has max length 20
            });
            _logger.LogInformation("Customer entity configuration completed.");
        }
    }
}