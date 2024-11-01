using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Shared;

namespace ProductSalesAPI.Application.Dashboard.Query.GetProductsHighestPr;

public record GetHighestTotalPriceSoldQuery() : IRequest<IEnumerable<ProductSalesResponse>>;
