using global::ProductSalesAPI.Application.ApiUtilities.Interfaces;
using global::ProductSalesAPI.Application.ApiUtilities.Shared;
using global::ProductSalesAPI.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ProductSalesAPI.Application.ApiUtilities.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public ProductService(
            IProductRepository productRepository,
            ILogger<ProductService> logger,
            IHubContext<NotificationsHub> hubContext)
        {
            _productRepository = productRepository;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<PagedResponse<Product>> GetPagedProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetPagedAsync(pageNumber, pageSize);
            var totalProducts = await _productRepository.GetCountAsync();

            return new PagedResponse<Product>(products, totalProducts, pageNumber, pageSize);
        }

        public async Task<int> CreateProductAsync(string name, decimal price)
        {
            var product = new Product(name, price);
            var productId = await _productRepository.AddAsync(product);
            _logger.LogInformation("Product created with ID {ProductId}.", productId);

            await _hubContext.Clients.All.SendAsync("ReceiveNewProduct", new { name, price });
            return productId;
        }
    }
}

