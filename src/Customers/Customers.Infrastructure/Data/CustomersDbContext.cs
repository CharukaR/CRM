using Microsoft.EntityFrameworkCore;
using Customers.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Customers.Infrastructure.Data
{
    public class CustomersDbContext : DbContext
    {
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
            ConfigureCustomerEntity(modelBuilder);
            _logger.LogInformation("OnModelCreating completed.");
        }

        private void ConfigureCustomerEntity(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("Configuring Customer entity.");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                _logger.LogDebug("Set primary key for Customer entity.");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                _logger.LogDebug("Configured Name property for Customer entity with max length 200 and required.");

                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                _logger.LogDebug("Configured Email property for Customer entity with max length 200 and required.");

                entity.Property(e => e.Phone).HasMaxLength(20);
                _logger.LogDebug("Configured Phone property for Customer entity with max length 20.");
            });

            _logger.LogInformation("Customer entity configuration completed.");
        }
    }
}