using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.SalesOrders.Query.RetrieveSalesOrders;

public record GetSalesOrdersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PagedResponse<SalesOrder>>;
