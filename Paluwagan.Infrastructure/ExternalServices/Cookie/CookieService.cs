using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Paluwagan.Domain.Services;
using Paluwagan.Infrastructure.Configurations;

namespace Paluwagan.Infrastructure.ExternalServices.Cookie
{
     public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SecurityOptions _securityOptions;

        public CookieService(IHttpContextAccessor httpContextAccessor, IOptions<SecurityOptions> securityOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityOptions = securityOptions.Value;
        }

        public string GetRefreshTokenFromCookie()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            return token ?? string.Empty;
        }

        public void SetRefreshTokenCookie(string refreshToken)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, BuildCookieOptions(expire: false));
        }

        public void DeleteRefreshTokenCookie()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", "", BuildCookieOptions(expire: true));
        }

        private CookieOptions BuildCookieOptions(bool expire)
        {
            var sameSite = Enum.TryParse<SameSiteMode>(_securityOptions.CookieSameSite, ignoreCase: true, out var parsed)
                ? parsed
                : SameSiteMode.Strict;

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = _securityOptions.RequireHttps,
                SameSite = sameSite,
                Path = "/",
                Expires = expire
                    ? DateTime.UtcNow.AddDays(-1)
                    : DateTime.UtcNow.AddDays(7),
                MaxAge = expire
                    ? TimeSpan.Zero
                    : TimeSpan.FromDays(7)
            };

            if (!string.IsNullOrWhiteSpace(_securityOptions.CookieDomain))
                options.Domain = _securityOptions.CookieDomain;

            return options;
        }
    }
}