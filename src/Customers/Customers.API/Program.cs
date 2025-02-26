using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging; // Import logging

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Add console logging for traceability

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
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

// Add DbContext
builder.Services.AddDbContext<CustomersDbContext>(options =>
{
    // Log the connection string being used (ensure sensitive data is not logged in production)
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Logging.CreateLogger("Startup").LogInformation("Using connection string: {ConnectionString}", connectionString);
    options.UseSqlServer(connectionString);
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));

// Add repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Log the environment the application is running in
app.Logger.LogInformation("Application starting in {Environment} environment", app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Log that the application has started successfully
app.Logger.LogInformation("Application started successfully and is running.");

app.Run();