using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;

namespace Customers.Infrastructure
{
    public static class DependencyInjection
    {
        // Method to add infrastructure services to the IServiceCollection
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Log the start of the infrastructure setup
            Console.WriteLine("Starting infrastructure setup.");

            // Configure the database context
            ConfigureDatabase(services, configuration);

            // Register repository implementations
            RegisterRepositories(services);

            // Log the completion of the infrastructure setup
            Console.WriteLine("Completed infrastructure setup.");

            return services;
        }

        // Method to configure the database context
        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            // Log the start of database configuration
            Console.WriteLine("Configuring database context.");

            // Add the CustomersDbContext with SQL Server configuration
            services.AddDbContext<CustomersDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(CustomersDbContext).Assembly.FullName)));

            // Log the completion of database configuration
            Console.WriteLine("Database context configured.");
        }

        // Method to register repository implementations
        private static void RegisterRepositories(IServiceCollection services)
        {
            // Log the start of repository registration
            Console.WriteLine("Registering repositories.");

            // Register the ICustomerRepository with its implementation CustomerRepository
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Log the completion of repository registration
            Console.WriteLine("Repositories registered.");
        }
    }
}