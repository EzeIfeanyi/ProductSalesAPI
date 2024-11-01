using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Application.ApiUtilities.Shared;

namespace ProductSalesAPI.Application.Dashboard.Query.GetProductsHighestQty;

public class GetHighestQuantitySoldQueryHandler : IRequestHandler<GetHighestQuantitySoldQuery, IEnumerable<ProductSalesResponse>>
{
    private readonly IDashboardService _dashboardService;

    public GetHighestQuantitySoldQueryHandler(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IEnumerable<ProductSalesResponse>> Handle(GetHighestQuantitySoldQuery request, CancellationToken cancellationToken)
    {
        return await _dashboardService.GetTopProductsByQuantitySold(5);
    }
}
