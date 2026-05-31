using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Application.DTOs
{
     public sealed record RefreshTokenResponse
    {
        public string AccessToken { get; init; }
        public Guid UserId { get; init; }
        
    
    }
}