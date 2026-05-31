using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Application.DTOs
{
    public class AuthResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public Guid UserId { get; init; }
    }
}