using ProductSalesAPI.Application.ApiUtilities.Shared;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<ProductSalesResponse>> GetTopProductsByQuantitySold(int topCount);
        Task<IEnumerable<ProductSalesResponse>> GetTopProductsByTotalPriceSold(int topCount);
    }
}