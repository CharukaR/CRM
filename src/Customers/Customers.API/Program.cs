using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure services and middleware
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure middleware components
ConfigureMiddleware(app);

// Start the application
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Add controllers to the service collection
    services.AddControllers();
    
    // Add API explorer for endpoint documentation
    services.AddEndpointsApiExplorer();
    
    // Configure Swagger for API documentation
    ConfigureSwagger(services);
    
    // Configure the database context
    ConfigureDatabase(services, configuration);
    
    // Register MediatR for handling commands
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
    
    // Register the customer repository for dependency injection
    services.AddScoped<ICustomerRepository, CustomerRepository>();
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
    // Configure the database context to use SQL Server with the connection string from configuration
    services.AddDbContext<CustomersDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

void ConfigureMiddleware(WebApplication app)
{
    // Check if the environment is development
    if (app.Environment.IsDevelopment())
    {
        // Enable Swagger and Swagger UI in development environment
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
    
    // Map controller routes
    app.MapControllers();
}