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

    public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand cmd)
    {
        _logger.LogInformation("Create method called with command: {@Command}", cmd);

        if (cmd?.n != null)
        {
            var result = await _mediator.Send(cmd);
            if (result != null)
            {
                _logger.LogInformation("Customer created successfully with ID: {CustomerId}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            else
            {
                _logger.LogWarning("Failed to create customer, result is null.");
            }
        }
        else
        {
            _logger.LogWarning("Invalid command received, command is null or invalid.");
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        _logger.LogInformation("GetById method called with ID: {CustomerId}", id);

        try
        {
            var query = new GetCustomerByIdQuery(id);
            var data = await _mediator.Send(query);
            if (data != null)
            {
                _logger.LogInformation("Customer found with ID: {CustomerId}", id);
                return Ok(data);
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
}