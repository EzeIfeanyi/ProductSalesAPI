using ProductSalesAPI.Application.ApiUtilities.Shared;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Application.ApiUtilities.Interfaces
{
    public interface IProductService
    {
        Task<int> CreateProductAsync(string name, decimal price);
        Task<PagedResponse<Product>> GetPagedProductsAsync(int pageNumber, int pageSize);
    }
}