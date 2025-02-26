using Microsoft.AspNetCore.Mvc;
using MediatR;
using Customers.Application.Commands;
using Customers.Application.Queries;
using Customers.Application.DTOs;

namespace Customers.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private static int _instanceCount = 0;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _instanceCount++;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand command)
    {
        if (command?.n == null)
        {
            return BadRequest();
        }

        var result = await _mediator.Send(command);
        if (result != null)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        try
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _mediator.Send(query);

            if (customer != null)
            {
                return Ok(customer);
            }
        }
        catch
        {
            // Log exception if necessary
        }

        return NotFound();
    }

    // Add other CRUD endpoints...
}