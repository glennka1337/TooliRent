using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var existing = await _users.GetByUsernameAsync(dto.Username);
            if (existing != null) throw new InvalidOperationException("Username already exists.");

            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Username = dto.Username,
                Password = hashed,
                Role = "Member"
            };
            await _users.AddAsync(user);
            return await GenerateTokenAsync(user);
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _users.GetByUsernameAsync(dto.Username);
            if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return await GenerateTokenAsync(user);
        }

        public async Task<AuthResultDto> RefreshAsync(string refreshToken)
        {
            var user = await _users.GetByRefreshTokenAsync(refreshToken);
            if (user == null) throw new UnauthorizedAccessException("Invalid refresh token.");
            if (!user.RefreshTokenExpiryUtc.HasValue || user.RefreshTokenExpiryUtc.Value < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expired.");

            return await GenerateTokenAsync(user);
        }

        public async Task LogoutAsync(int userId)
        {
            var user = await _users.GetByIdAsync(userId) ?? throw new KeyNotFoundException("User not found.");
            user.RefreshToken = null;
            user.RefreshTokenExpiryUtc = null;
            await _users.UpdateAsync(user);
        }

        private async Task<AuthResultDto> GenerateTokenAsync(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiryMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("uid", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            // create refresh token
            var refresh = Guid.NewGuid().ToString("N");
            var refreshExpiry = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = refresh;
            user.RefreshTokenExpiryUtc = refreshExpiry;
            await _users.UpdateAsync(user);

            return new AuthResultDto(tokenString, expires, user.Username, user.Role, refresh, refreshExpiry);
        }
    }
}