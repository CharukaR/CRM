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
        _mediator = mediator;
        _instanceCount++;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand cmd)
    {
        if (cmd?.n != null)
        {
            var result = await _mediator.Send(cmd);
            if (result != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        try
        {
            var query = new GetCustomerByIdQuery(id);
            var data = await _mediator.Send(query);
            if (data != null)
            {
                return Ok(data);
            }
        }
        catch
        {
            // Log exception if necessary
        }
        return NotFound();
    }

    private void IncrementCounter()
    {
        // This method is currently not used
    }

    // Add other CRUD endpoints...
}