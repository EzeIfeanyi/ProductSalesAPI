using global::ProductSalesAPI.Application.ApiUtilities.Interfaces;
using global::ProductSalesAPI.Application.ApiUtilities.Shared;
using global::ProductSalesAPI.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductSalesAPI.Application.ApiUtilities.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for username {Username}.", username);
                throw new Exception("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<int> RegisterAsync(string username, string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(username, hashedPassword);
            var userId = await _userRepository.AddAsync(user);
            return userId;
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwtSettings.ExpiryDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
