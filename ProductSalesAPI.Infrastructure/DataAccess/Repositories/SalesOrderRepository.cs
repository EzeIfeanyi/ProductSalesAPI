using Dapper;
using Microsoft.Extensions.Logging;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Infrastructure.DataAccess.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<SalesOrderRepository> _logger;

        public SalesOrderRepository(IDatabaseContext dbContext, ILogger<SalesOrderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(SalesOrder salesOrder)
        {
            var sql = """
                    INSERT INTO SalesOrders (ProductId, CustomerId, Quantity, OrderDate) 
                    VALUES (@ProductId, @CustomerId, @Quantity, @OrderDate) RETURNING Id;
                """;
            var salesOrderId = await _dbContext.Connection.ExecuteScalarAsync<int>(sql, salesOrder);
            _logger.LogInformation("Sales order created with ID {SalesOrderId}.", salesOrderId);
            return salesOrderId;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllAsync()
        {
            var sql = "SELECT * FROM SalesOrders;";
            var salesOrders = await _dbContext.Connection.QueryAsync<SalesOrder>(sql);
            _logger.LogInformation("Retrieved {Count} sales orders.", salesOrders.Count());
            return salesOrders;
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM SalesOrders WHERE Id = @Id;";
            await _dbContext.Connection.ExecuteAsync(sql, new { Id = id });
            _logger.LogInformation("Sales order with ID {SalesOrderId} deleted.", id);
        }

        public async Task<IEnumerable<SalesOrder>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var sql = "SELECT * FROM SalesOrders ORDER BY Id OFFSET @Offset LIMIT @Limit;";
            return await _dbContext.Connection.QueryAsync<SalesOrder>(sql, new { Offset = (pageNumber - 1) * pageSize, Limit = pageSize });
        }

        public async Task<int> GetCountAsync()
        {
            var sql = "SELECT COUNT(*) FROM SalesOrders;";
            return await _dbContext.Connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
