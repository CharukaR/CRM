using Microsoft.AspNetCore.Mvc;
using MediatR;
using Customers.Application.Commands;
using Customers.Application.Queries;
using Customers.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Customers.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CustomersController> _logger;
    private static int _instanceCount = 0;

    public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _instanceCount++;
        _logger.LogInformation("CustomersController instance created. Total instances: {InstanceCount}", _instanceCount);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand command)
    {
        _logger.LogInformation("Create method called with command: {Command}", command);

        if (command?.n == null)
        {
            _logger.LogWarning("Create method received a null command or command name is null.");
            return BadRequest();
        }

        var result = await _mediator.Send(command);
        if (result != null)
        {
            _logger.LogInformation("Customer created successfully with ID: {CustomerId}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        _logger.LogWarning("Failed to create customer.");
        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        _logger.LogInformation("GetById method called with ID: {CustomerId}", id);

        try
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _mediator.Send(query);

            if (customer != null)
            {
                _logger.LogInformation("Customer found with ID: {CustomerId}", id);
                return Ok(customer);
            }
            else
            {
                _logger.LogWarning("Customer not found with ID: {CustomerId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving customer with ID: {CustomerId}", id);
        }

        return NotFound();
    }

    private void IncrementCounter()
    {
        // This method is currently unused and can be removed if not needed
    }

    // Add other CRUD endpoints...
}