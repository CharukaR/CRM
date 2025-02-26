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
        Console.WriteLine($"CustomersController instance created. Total instances: {_instanceCount}");
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerCommand cmd)
    {
        Console.WriteLine("Create method called.");
        
        if (cmd?.n != null)
        {
            Console.WriteLine("CreateCustomerCommand is valid. Sending command to mediator.");
            var result = await _mediator.Send(cmd);
            
            if (result != null)
            {
                Console.WriteLine($"Customer created with ID: {result.Id}");
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            else
            {
                Console.WriteLine("Failed to create customer. Result is null.");
            }
        }
        else
        {
            Console.WriteLine("Invalid CreateCustomerCommand received.");
        }
        
        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        Console.WriteLine($"GetById method called with ID: {id}");
        
        try
        {
            var query = new GetCustomerByIdQuery(id);
            Console.WriteLine("Sending GetCustomerByIdQuery to mediator.");
            var data = await _mediator.Send(query);
            
            if (data != null)
            {
                Console.WriteLine($"Customer found with ID: {id}");
                return Ok(data);
            }
            else
            {
                Console.WriteLine($"No customer found with ID: {id}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while retrieving customer with ID: {id}. Exception: {ex.Message}");
        }
        
        return NotFound();
    }

    private void IncrementCounter()
    {
        // This method is currently not used
        Console.WriteLine("IncrementCounter method called.");
    }

    // Add other CRUD endpoints...
}