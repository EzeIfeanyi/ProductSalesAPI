using MediatR;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Application.ApiUtilities.Shared;

namespace ProductSalesAPI.Application.Dashboard.Query.GetProductsHighestPr;

public class GetHighestTotalPriceSoldQueryHandler : IRequestHandler<GetHighestTotalPriceSoldQuery, IEnumerable<ProductSalesResponse>>
{
    private readonly IDashboardService _dashboardService;

    public GetHighestTotalPriceSoldQueryHandler(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IEnumerable<ProductSalesResponse>> Handle(GetHighestTotalPriceSoldQuery request, CancellationToken cancellationToken)
    {
        return await _dashboardService.GetTopProductsByTotalPriceSold(5);
    }
}
