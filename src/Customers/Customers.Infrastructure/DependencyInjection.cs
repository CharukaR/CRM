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

            // Configure the DbContext with SQL Server and set the migrations assembly
            services.AddDbContext<CustomersDbContext>(options =>
            {
                Console.WriteLine("Configuring DbContext with SQL Server.");
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(CustomersDbContext).Assembly.FullName));
            });

            // Register the CustomerRepository as a scoped service for ICustomerRepository
            Console.WriteLine("Registering ICustomerRepository with CustomerRepository.");
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            // Log the end of the AddInfrastructure method
            Console.WriteLine("Completed AddInfrastructure method.");

            return services;
        }
    }
}