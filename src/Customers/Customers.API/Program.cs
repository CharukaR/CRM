using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure services and dependencies
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure middleware for the application
ConfigureMiddleware(app);

// Start the application
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Add controllers to the service collection
    services.AddControllers();
    services.AddEndpointsApiExplorer();

    // Configure Swagger for API documentation
    ConfigureSwagger(services);

    // Configure the database context
    ConfigureDatabase(services, configuration);

    // Configure MediatR for handling commands and queries
    ConfigureMediatR(services);

    // Configure repositories for dependency injection
    ConfigureRepositories(services);
}

void ConfigureSwagger(IServiceCollection services)
{
    // Add Swagger generation with basic API information
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CRM API",
            Version = "v1",
            Description = "API for managing customers in the CRM system",
            Contact = new OpenApiContact
            {
                Name = "CRM Support",
                Email = "support@crm.com"
            }
        });
    });
}

void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    // Configure the database context to use SQL Server with a connection string from configuration
    services.AddDbContext<CustomersDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

void ConfigureMediatR(IServiceCollection services)
{
    // Register MediatR services from the assembly containing CreateCustomerCommand
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
}

void ConfigureRepositories(IServiceCollection services)
{
    // Register the customer repository for dependency injection
    services.AddScoped<ICustomerRepository, CustomerRepository>();
}

void ConfigureMiddleware(WebApplication app)
{
    // Check if the environment is development to enable Swagger UI
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
            c.RoutePrefix = string.Empty;
        });
    }

    // Enable HTTPS redirection
    app.UseHttpsRedirection();

    // Enable authorization middleware
    app.UseAuthorization();

    // Map controller endpoints
    app.MapControllers();
}