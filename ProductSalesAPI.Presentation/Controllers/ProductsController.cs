using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Application.Products.Command.CreateProduct;
using ProductSalesAPI.Application.Products.Query.RetrieveProducts;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetProductsQuery(pageNumber, pageSize));
        _logger.LogInformation("Products retrieved successfully. Count: {Count}", result.TotalCount);
        return Ok(new ApiResponse<PagedResponse<Product>>(true, "Products retrieved successfully.", result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        _logger.LogInformation("Product created with ID {ProductId}.", productId);
        return CreatedAtAction(nameof(GetProducts), new { id = productId }, new ApiResponse<int>(true, "Product created successfully.", productId));
    }
}
