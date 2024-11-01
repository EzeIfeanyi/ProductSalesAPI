using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces;

public interface ISalesOrderRepository
{
    Task<int> AddAsync(SalesOrder salesOrder);
    Task<IEnumerable<SalesOrder>> GetAllAsync();
    Task DeleteAsync(int id);
    Task<IEnumerable<SalesOrder>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetCountAsync();
}
