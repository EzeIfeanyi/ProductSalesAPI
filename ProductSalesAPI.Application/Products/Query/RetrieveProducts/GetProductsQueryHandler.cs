using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.Products.Query.RetrieveProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResponse<Product>>
{
    private readonly IProductService _productService;

    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<PagedResponse<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        return await _productService.GetPagedProductsAsync(pageNumber, pageSize);
    }
}
