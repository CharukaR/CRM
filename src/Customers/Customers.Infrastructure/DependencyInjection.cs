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
        // Method to configure and add infrastructure services to the IServiceCollection
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Log the start of the AddInfrastructure method
            Console.WriteLine("Starting AddInfrastructure method.");

            // Configure the DbContext with SQL Server and specify the migrations assembly
            services.AddDbContext<CustomersDbContext>(options =>
            {
                // Log the connection string being used
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Configuring DbContext with connection string: {connectionString}");

                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(CustomersDbContext).Assembly.FullName));
            });

            // Register the CustomerRepository as the implementation for ICustomerRepository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            Console.WriteLine("Registered ICustomerRepository with CustomerRepository.");

            // Log the end of the AddInfrastructure method
            Console.WriteLine("Completed AddInfrastructure method.");

            return services;
        }
    }
}