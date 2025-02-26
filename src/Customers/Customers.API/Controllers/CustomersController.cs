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
        // Log the creation of a new instance of CustomersController
        Console.WriteLine("CustomersController instance created.");
        
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _instanceCount++;
        
        // Log the current instance count
        Console.WriteLine($"Current CustomersController instance count: {_instanceCount}");
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand cmd)
    {
        // Log the start of the Create method
        Console.WriteLine("Create method called.");

        if (cmd?.n == null)
        {
            // Log the reason for bad request
            Console.WriteLine("Create method: BadRequest due to null command or command name.");
            return BadRequest();
        }

        // Log the command details
        Console.WriteLine($"Create method: Processing command with name: {cmd.n}");

        var result = await _mediator.Send(cmd);
        
        if (result != null)
        {
            // Log successful creation
            Console.WriteLine($"Create method: Customer created with ID: {result.Id}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // Log failure to create customer
        Console.WriteLine("Create method: Failed to create customer.");
        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        // Log the start of the GetById method
        Console.WriteLine($"GetById method called with ID: {id}");

        try
        {
            var query = new GetCustomerByIdQuery(id);
            var data = await _mediator.Send(query);

            if (data != null)
            {
                // Log successful retrieval
                Console.WriteLine($"GetById method: Customer found with ID: {id}");
                return Ok(data);
            }
        }
        catch (Exception ex)
        {
            // Log exception details
            Console.WriteLine($"GetById method: Exception occurred - {ex.Message}");
        }

        // Log customer not found
        Console.WriteLine($"GetById method: Customer not found with ID: {id}");
        return NotFound();
    }

    // Add other CRUD endpoints...
}