using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Starting application setup...");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    Console.WriteLine("Configuring Swagger for API documentation...");
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
    Console.WriteLine("Swagger configuration completed.");
});

// Add DbContext for database operations
builder.Services.AddDbContext<CustomersDbContext>(options =>
{
    Console.WriteLine("Configuring DbContext with SQL Server...");
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    Console.WriteLine("DbContext configuration completed.");
});

// Add MediatR for handling commands and queries
builder.Services.AddMediatR(cfg =>
{
    Console.WriteLine("Registering MediatR services...");
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
    Console.WriteLine("MediatR services registration completed.");
});

// Add repositories for data access
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
Console.WriteLine("Repository services added.");

var app = builder.Build();
Console.WriteLine("Application build completed.");

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Development environment detected. Configuring Swagger UI...");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
    Console.WriteLine("Swagger UI configuration completed.");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("Starting application...");
app.Run();
Console.WriteLine("Application is running.");