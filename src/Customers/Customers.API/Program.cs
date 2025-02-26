using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add logging
var logger = builder.Logging.AddConsole().CreateLogger("Startup");

logger.LogInformation("Starting application setup.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
logger.LogInformation("Configuring Swagger.");
ConfigureSwagger(builder.Services);

// Add DbContext
logger.LogInformation("Configuring DbContext.");
ConfigureDbContext(builder.Services, builder.Configuration);

// Add MediatR
logger.LogInformation("Configuring MediatR.");
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));

// Add repositories
logger.LogInformation("Adding repositories.");
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    logger.LogInformation("Environment is Development. Configuring Swagger UI.");
    ConfigureSwaggerUI(app);
}

logger.LogInformation("Configuring middleware.");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

logger.LogInformation("Running the application.");
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
    logger.LogInformation("Swagger configured.");
}

void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<CustomersDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    logger.LogInformation("DbContext configured with SQL Server.");
}

void ConfigureSwaggerUI(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
    logger.LogInformation("Swagger UI configured.");
}