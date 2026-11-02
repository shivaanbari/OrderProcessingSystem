using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Orders.Commands;
using OrderService.Application.Features.Orders.Queries;

namespace OrderService.API.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var orderId = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { id = orderId },
            new { orderId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrderById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);

        var order = await _mediator.Send(
            query,
            cancellationToken);

        if (order == null)
            return NotFound();

        return Ok(order);
    }
}