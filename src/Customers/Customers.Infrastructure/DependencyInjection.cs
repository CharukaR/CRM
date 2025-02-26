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
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Log the start of the infrastructure setup
            Console.WriteLine("Starting infrastructure setup...");

            // Configure the database context
            ConfigureDatabase(services, configuration);

            // Register the repositories
            RegisterRepositories(services);

            // Log the completion of the infrastructure setup
            Console.WriteLine("Infrastructure setup completed.");

            return services;
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            // Log the start of database configuration
            Console.WriteLine("Configuring database...");

            // Configure the DbContext with SQL Server and specify the migrations assembly
            services.AddDbContext<CustomersDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(CustomersDbContext).Assembly.FullName)));

            // Log the completion of database configuration
            Console.WriteLine("Database configured successfully.");
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            // Log the start of repository registration
            Console.WriteLine("Registering repositories...");

            // Register the customer repository with scoped lifetime
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Log the completion of repository registration
            Console.WriteLine("Repositories registered successfully.");
        }
    }
}