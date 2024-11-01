using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Application.SalesOrders.Command.CreateSalesOrder;
using ProductSalesAPI.Application.SalesOrders.Command.DeleteSalesOrder;
using ProductSalesAPI.Application.SalesOrders.Query;
using ProductSalesAPI.Application.SalesOrders.Query.RetrieveSalesOrders;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetSalesOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetSalesOrdersQuery(pageNumber, pageSize));
        _logger.LogInformation("Sales orders retrieved successfully. Count: {Count}", result.TotalCount);
        return Ok(new ApiResponse<PagedResponse<SalesOrder>>(true, "Sales orders retrieved successfully.", result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderCommand command)
    {
        var salesOrderId = await _mediator.Send(command);
        _logger.LogInformation("Sales order created with ID {SalesOrderId}.", salesOrderId);
        return CreatedAtAction(
            nameof(GetSalesOrders),
            new { id = salesOrderId },
            new ApiResponse<int>(true, "Sales order created successfully.", salesOrderId)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesOrder(int id)
    {
        await _mediator.Send(new DeleteSalesOrderCommand(id));
        _logger.LogInformation("Sales order with ID {SalesOrderId} deleted successfully.", id);
        return Ok(new ApiResponse<int>(true, "Sales order deleted successfully.", id));
    }
}
