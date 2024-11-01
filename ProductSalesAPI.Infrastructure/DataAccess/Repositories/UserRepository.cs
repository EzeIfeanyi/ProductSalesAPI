using Dapper;
using Microsoft.Extensions.Logging;
using ProductSalesAPI.Application.ApiUtilities.Interfaces;
using ProductSalesAPI.Domain.Entities;

namespace ProductSalesAPI.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseContext _dbContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IDatabaseContext dbContext, ILogger<UserRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(User user)
        {
            var sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash) RETURNING Id;";
            var userId = await _dbContext.Connection.ExecuteScalarAsync<int>(sql, user);
            _logger.LogInformation("User {Username} added with ID {UserId}.", user.Username, userId);
            return userId;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username;";
            var user = await _dbContext.Connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
            _logger.LogInformation("Retrieved user {Username}.", username);
            return user;
        }
    }
}
