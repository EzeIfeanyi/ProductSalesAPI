using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Application.Dashboard.Query.GetProductsHighestPr;
using ProductSalesAPI.Application.Dashboard.Query.GetProductsHighestQty;

namespace ProductSalesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IMediator mediator, ILogger<DashboardController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("highest-quantity-sold")]
    public async Task<IActionResult> GetHighestQuantitySold()
    {
        var result = await _mediator.Send(new GetHighestQuantitySoldQuery());
        _logger.LogInformation("Retrieved highest quantity sold products: Count {Count}", result?.Count());
        return Ok(new ApiResponse<IEnumerable<ProductSalesResponse>>(true, "Highest quantity sold products retrieved successfully.", result));
    }

    [HttpGet("highest-total-price-sold")]
    public async Task<IActionResult> GetHighestTotalPriceSold()
    {
        var result = await _mediator.Send(new GetHighestTotalPriceSoldQuery());
        _logger.LogInformation("Retrieved highest total price sold products: Count {Count}", result?.Count());
        return Ok(new ApiResponse<IEnumerable<ProductSalesResponse>>(true, "Highest total price sold products retrieved successfully.", result));
    }
}
