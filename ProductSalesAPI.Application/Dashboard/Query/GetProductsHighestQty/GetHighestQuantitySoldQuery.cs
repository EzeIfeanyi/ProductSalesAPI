using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Shared;

namespace ProductSalesAPI.Application.Dashboard.Query.GetProductsHighestQty;

public record GetHighestQuantitySoldQuery() : IRequest<IEnumerable<ProductSalesResponse>>;
