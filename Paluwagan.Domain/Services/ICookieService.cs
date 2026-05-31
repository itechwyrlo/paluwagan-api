using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Domain.Services
{
    public interface ICookieService
    {
        string GetRefreshTokenFromCookie();
        void SetRefreshTokenCookie(string refreshToken);
        void DeleteRefreshTokenCookie();
    }
}