using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces;

public interface IProductRepository
{
    Task<int> AddAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize);
    Task<int> GetCountAsync();
}
