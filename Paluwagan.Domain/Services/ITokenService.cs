using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Domain.Entities;

namespace Paluwagan.Domain.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user, string role);

        string GenerateRefreshTokenRaw();
        string HashToken(string raw);
        bool IsJtiBlacklisted(string jti);
        Task BlacklistJtiAsync(string jti, CancellationToken ct = default);
    }
}