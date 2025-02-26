using Microsoft.EntityFrameworkCore;
using Customers.Application.Commands;
using Customers.Domain.Interfaces;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    ConfigureSwagger(services);
    ConfigureDatabase(services, configuration);
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
    services.AddScoped<ICustomerRepository, CustomerRepository>();
}

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

void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<CustomersDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API V1");
            c.RoutePrefix = string.Empty;
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}