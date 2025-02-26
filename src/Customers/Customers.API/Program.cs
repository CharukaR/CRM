using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Log the configuration of Swagger
    builder.Logging.CreateLogger("Startup").LogInformation("Configuring Swagger");
    
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

// Add DbContext
builder.Services.AddDbContext<CustomersDbContext>(options =>
{
    // Log the configuration of DbContext
    builder.Logging.CreateLogger("Startup").LogInformation("Configuring DbContext with SQL Server");
    
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    // Log the registration of MediatR services
    builder.Logging.CreateLogger("Startup").LogInformation("Registering MediatR services");
    
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
});

// Add repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Log the environment
builder.Logging.CreateLogger("Startup").LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
    // Log the use of Swagger in development
    builder.Logging.CreateLogger("Startup").LogInformation("Using Swagger in Development environment");
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Log the mapping of controllers
builder.Logging.CreateLogger("Startup").LogInformation("Mapping controllers");

app.MapControllers();

// Log the start of the application
builder.Logging.CreateLogger("Startup").LogInformation("Running the application");

app.Run();