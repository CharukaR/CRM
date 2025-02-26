using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
ConfigureSwagger(builder.Services);

// Add DbContext
ConfigureDbContext(builder.Services, builder.Configuration);

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));

// Add repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    ConfigureSwaggerUI(app);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

void ConfigureSwagger(IServiceCollection services)
{
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

void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<CustomersDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

void ConfigureSwaggerUI(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
}