using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.SalesOrders.Query.RetrieveSalesOrders;

public class GetSalesOrdersQueryHandler : IRequestHandler<GetSalesOrdersQuery, PagedResponse<SalesOrder>>
{
    private readonly ISalesOrderService _salesOrderService;

    public GetSalesOrdersQueryHandler(ISalesOrderService salesOrderService)
    {
        _salesOrderService = salesOrderService;
    }

    public async Task<PagedResponse<SalesOrder>> Handle(GetSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        return await _salesOrderService.GetPagedSalesOrdersAsync(pageNumber, pageSize);
    }
}
