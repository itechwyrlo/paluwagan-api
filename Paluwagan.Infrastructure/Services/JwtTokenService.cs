using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Paluwagan.Domain.Entities;
using Paluwagan.Domain.Services;
using Paluwagan.Infrastructure.Configurations;

namespace Paluwagan.Infrastructure.Services
{
    public sealed class JwtTokenService(IOptions<JwtSettings> opts, IMemoryCache cache) : ITokenService
    {
        private readonly JwtSettings _opts = opts.Value;
        private readonly IMemoryCache _cache = cache;

        public string GenerateAccessToken(ApplicationUser user, string role)
        {
            var jti = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Jti, jti)
            };

            return WriteToken(claims);
        }

        public string GenerateRefreshTokenRaw()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public string HashToken(string raw)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes).ToLower();
        }

        public bool IsJtiBlacklisted(string jti)
        {
            return _cache.TryGetValue($"jti:{jti}", out _);
        }

        public Task BlacklistJtiAsync(string jti, CancellationToken ct)
        {
            var expiry = TimeSpan.FromMinutes(_opts.ExpiryMinutes + 1);
            _cache.Set($"jti:{jti}", "1", expiry);
            return Task.CompletedTask;
        }



        private string WriteToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_opts.ExpiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}