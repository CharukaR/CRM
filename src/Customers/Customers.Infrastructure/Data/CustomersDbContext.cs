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

        // Configures the schema needed for the Customer entity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation("OnModelCreating started.");

            modelBuilder.Entity<Customer>(entity =>
            {
                _logger.LogDebug("Configuring Customer entity.");

                // Setting the primary key for the Customer entity
                entity.HasKey(e => e.Id);
                _logger.LogDebug("Primary key configured for Customer entity.");

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