using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Customers.Infrastructure.Data
{
    public class CustomersDbContext : DbContext
    {
        // Logger to trace the execution flow and state changes
        private readonly ILogger<CustomersDbContext> _logger;

        public CustomersDbContext(DbContextOptions<CustomersDbContext> options, ILogger<CustomersDbContext> logger) : base(options)
        {
            _logger = logger;
            _logger.LogInformation("CustomersDbContext initialized with options.");
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("OnModelCreating started.");

            modelBuilder.Entity<Customer>(entity =>
            {
                _logger.LogInformation("Configuring Customer entity.");

                // Setting the primary key for the Customer entity
                entity.HasKey(e => e.Id);
                _logger.LogDebug("Primary key set for Customer entity.");

                // Configuring properties with constraints
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                _logger.LogDebug("Name property configured with IsRequired and MaxLength(200).");

                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                _logger.LogDebug("Email property configured with IsRequired and MaxLength(200).");

                entity.Property(e => e.Phone).HasMaxLength(20);
                _logger.LogDebug("Phone property configured with MaxLength(20).");
            });

            _logger.LogInformation("OnModelCreating completed.");
        }
    }
}