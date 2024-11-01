using Dapper;
using Microsoft.Extensions.Logging;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;

namespace ProductSalesAPI.Infrastructure.DataAccess
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(IDatabaseContext dbContext, ILogger<DatabaseInitializer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Initializing the database...");

            await CreateUsersTableAsync();
            await CreateProductsTableAsync();
            await CreateSalesOrdersTableAsync();
            await SeedDataAsync();

            _logger.LogInformation("Database initialization completed.");
        }

        private async Task CreateUsersTableAsync()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Username VARCHAR(50) NOT NULL,
                    PasswordHash VARCHAR(255) NOT NULL
                );";
            await _dbContext.Connection.ExecuteAsync(sql);
            _logger.LogInformation("Users table created or already exists.");
        }

        private async Task CreateProductsTableAsync()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(100) NOT NULL,
                    Price DECIMAL(18, 2) NOT NULL
                );";
            await _dbContext.Connection.ExecuteAsync(sql);
            _logger.LogInformation("Products table created or already exists.");
        }

        private async Task CreateSalesOrdersTableAsync()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS SalesOrders (
                    Id SERIAL PRIMARY KEY,
                    ProductId INT NOT NULL,
                    CustomerId INT NOT NULL,
                    Quantity INT NOT NULL,
                    OrderDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (ProductId) REFERENCES Products(Id)
                );";
            await _dbContext.Connection.ExecuteAsync(sql);
            _logger.LogInformation("SalesOrders table created or already exists.");
        }

        private async Task SeedDataAsync()
        {
            _logger.LogInformation("Seeding data into the database...");
            await SeedUsersAsync();
            await SeedProductsAsync();
            await SeedSalesOrdersAsync();
            _logger.LogInformation("Data seeding completed.");
        }

        private async Task SeedUsersAsync()
        {
            var sql = "SELECT COUNT(*) FROM Users;";
            var userCount = await _dbContext.Connection.ExecuteScalarAsync<int>(sql);

            if (userCount == 0)
            {
                var insertUserSql = @"
                    INSERT INTO Users (Username, PasswordHash) 
                    VALUES (@Username, @PasswordHash);";
                var defaultUsers = new[]
                {
                    new { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") },
                    new { Username = "user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") }
                };
                await _dbContext.Connection.ExecuteAsync(insertUserSql, defaultUsers);
                _logger.LogInformation("Default users seeded into the database.");
            }
            else
            {
                _logger.LogInformation("Users already exist in the database. Skipping seeding.");
            }
        }

        private async Task SeedProductsAsync()
        {
            var sql = "SELECT COUNT(*) FROM Products;";
            var productCount = await _dbContext.Connection.ExecuteScalarAsync<int>(sql);

            if (productCount == 0)
            {
                var insertProductSql = @"
                    INSERT INTO Products (Name, Price) 
                    VALUES (@Name, @Price);";
                var defaultProducts = new[]
                {
                    new { Name = "Product A", Price = 19.99m },
                    new { Name = "Product B", Price = 29.99m }
                };
                await _dbContext.Connection.ExecuteAsync(insertProductSql, defaultProducts);
                _logger.LogInformation("Default products seeded into the database.");
            }
            else
            {
                _logger.LogInformation("Products already exist in the database. Skipping seeding.");
            }
        }

        private async Task SeedSalesOrdersAsync()
        {
            var sql = "SELECT COUNT(*) FROM SalesOrders;";
            var salesOrderCount = await _dbContext.Connection.ExecuteScalarAsync<int>(sql);

            if (salesOrderCount == 0)
            {
                var insertSalesOrderSql = @"
                    INSERT INTO SalesOrders (ProductId, CustomerId, Quantity, OrderDate) 
                    VALUES (@ProductId, @CustomerId, @Quantity, @OrderDate);";
                var defaultSalesOrders = new[]
                {
                    new { ProductId = 1, CustomerId = 1, Quantity = 5, OrderDate = DateTime.UtcNow },
                    new { ProductId = 2, CustomerId = 2, Quantity = 3, OrderDate = DateTime.UtcNow }
                };
                await _dbContext.Connection.ExecuteAsync(insertSalesOrderSql, defaultSalesOrders);
                _logger.LogInformation("Default sales orders seeded into the database.");
            }
            else
            {
                _logger.LogInformation("Sales orders already exist in the database. Skipping seeding.");
            }
        }
    }
}
