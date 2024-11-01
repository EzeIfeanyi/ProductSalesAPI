using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.Products.Query.RetrieveProducts;

public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<Product>>;
