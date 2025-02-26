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
            Console.WriteLine("Starting AddInfrastructure method.");

            // Configure the DbContext with SQL Server and set the migrations assembly
            Console.WriteLine("Configuring DbContext with SQL Server.");
            services.AddDbContext<CustomersDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(CustomersDbContext).Assembly.FullName)));

            // Register the CustomerRepository as a scoped service
            Console.WriteLine("Registering ICustomerRepository with CustomerRepository.");
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            Console.WriteLine("AddInfrastructure method completed.");
            return services;
        }
    }
}