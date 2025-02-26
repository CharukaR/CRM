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
        Console.WriteLine("Creating instance of CustomersController");
        
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _instanceCount++;
        
        // Log the current instance count
        Console.WriteLine($"CustomersController instance count: {_instanceCount}");
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand command)
    {
        // Log the start of the Create method
        Console.WriteLine("Starting Create method");

        if (command?.n == null)
        {
            // Log the bad request due to null command
            Console.WriteLine("Create method received a null command");
            return BadRequest();
        }

        // Log the command details
        Console.WriteLine($"Received CreateCustomerCommand: {command}");

        var result = await _mediator.Send(command);

        if (result != null)
        {
            // Log successful creation of customer
            Console.WriteLine($"Customer created with ID: {result.Id}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // Log the failure to create a customer
        Console.WriteLine("Failed to create customer");
        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        // Log the start of the GetById method
        Console.WriteLine($"Starting GetById method for ID: {id}");

        try
        {
            var query = new GetCustomerByIdQuery(id);
            // Log the query details
            Console.WriteLine($"Executing GetCustomerByIdQuery for ID: {id}");

            var customer = await _mediator.Send(query);

            if (customer != null)
            {
                // Log successful retrieval of customer
                Console.WriteLine($"Customer found with ID: {id}");
                return Ok(customer);
            }
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.WriteLine($"Exception occurred in GetById method: {ex.Message}");
        }

        // Log the customer not found scenario
        Console.WriteLine($"Customer not found with ID: {id}");
        return NotFound();
    }

    // Add other CRUD endpoints...
}