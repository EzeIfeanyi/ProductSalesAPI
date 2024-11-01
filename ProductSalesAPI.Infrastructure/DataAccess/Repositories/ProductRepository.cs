using Dapper;
using Microsoft.Extensions.Logging;
using ProductSalesAPI.Domain.Entities;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;

namespace ProductSalesAPI.Infrastructure.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IDatabaseContext dbContext, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(Product product)
        {
            var sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price) RETURNING Id;";
            var productId = await _dbContext.Connection.ExecuteScalarAsync<int>(sql, product);
            _logger.LogInformation("Product created with ID {ProductId}.", productId);
            return productId;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products;";
            var products = await _dbContext.Connection.QueryAsync<Product>(sql);
            _logger.LogInformation("Retrieved {Count} products.", products.Count());
            return products;
        }

        public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var sql = "SELECT * FROM Products ORDER BY Id OFFSET @Offset LIMIT @Limit;";
            return await _dbContext.Connection.QueryAsync<Product>(sql, new { Offset = (pageNumber - 1) * pageSize, Limit = pageSize });
        }

        public async Task<int> GetCountAsync()
        {
            var sql = "SELECT COUNT(*) FROM Products;";
            return await _dbContext.Connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
