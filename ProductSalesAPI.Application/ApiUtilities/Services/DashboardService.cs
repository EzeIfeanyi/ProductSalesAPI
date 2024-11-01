using global::ProductSalesAPI.Application.ApiUtilities.Interfaces;
using global::ProductSalesAPI.Application.ApiUtilities.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductSalesAPI.Application.ApiUtilities.Services
{
    public class DashboardService(ISalesOrderRepository salesOrderRepository, IProductRepository productRepository) : IDashboardService
    {
        private readonly ISalesOrderRepository _salesOrderRepository = salesOrderRepository;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<ProductSalesResponse>> GetTopProductsByTotalPriceSold(int topCount)
        {
            var salesOrders = await _salesOrderRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            var groupedSales = salesOrders
                .GroupBy(so => so.ProductId)
                .Select(g => new ProductSalesResponse(
                    g.Key,
                    products.First(p => p.Id == g.Key).Name,
                    g.Sum(so => so.Quantity),
                    g.Sum(so => so.Quantity * products.First(p => p.Id == g.Key).Price)
                ));

            return groupedSales.OrderByDescending(p => p.TotalPriceSold).Take(topCount);
        }

        public async Task<IEnumerable<ProductSalesResponse>> GetTopProductsByQuantitySold(int topCount)
        {
            var salesOrders = await _salesOrderRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            var groupedSales = salesOrders
                .GroupBy(so => so.ProductId)
                .Select(g => new ProductSalesResponse(
                    g.Key,
                    products.First(p => p.Id == g.Key).Name,
                    g.Sum(so => so.Quantity),
                    g.Sum(so => so.Quantity * products.First(p => p.Id == g.Key).Price)
                ));

            return groupedSales.OrderByDescending(p => p.TotalQuantitySold).Take(topCount);
        }
    }
}

