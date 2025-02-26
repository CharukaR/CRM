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

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand cmd)
    {
        Console.WriteLine("Create method called with command: {0}", cmd);

        if (cmd?.n != null)
        {
            Console.WriteLine("Command is valid, sending to mediator.");
            var result = await _mediator.Send(cmd);
            if (result != null)
            {
                Console.WriteLine("Customer created successfully with ID: {0}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            else
            {
                Console.WriteLine("Failed to create customer.");
            }
        }
        else
        {
            Console.WriteLine("Invalid command received.");
        }

        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        Console.WriteLine("GetById method called with ID: {0}", id);

        try
        {
            var query = new GetCustomerByIdQuery(id);
            Console.WriteLine("Sending query to mediator.");
            var data = await _mediator.Send(query);
            if (data != null)
            {
                Console.WriteLine("Customer found with ID: {0}", id);
                return Ok(data);
            }
            else
            {
                Console.WriteLine("Customer not found with ID: {0}", id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred while retrieving customer: {0}", ex.Message);
            // Log exception if necessary
        }

        return NotFound();
    }
}