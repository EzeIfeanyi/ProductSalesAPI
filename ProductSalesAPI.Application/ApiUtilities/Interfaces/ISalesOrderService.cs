using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces
{
    public interface ISalesOrderService
    {
        Task<int> CreateSalesOrderAsync(int productId, int customerId, int quantity);
        Task DeleteSalesOrderAsync(int salesOrderId);
        Task<PagedResponse<SalesOrder>> GetPagedSalesOrdersAsync(int pageNumber, int pageSize);
    }
}