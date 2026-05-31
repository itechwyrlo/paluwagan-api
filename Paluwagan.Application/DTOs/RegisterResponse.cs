using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paluwagan.Application.DTOs
{
    public class RegisterResponse
    {
        public Guid UserId { get; init; }
        public string AccountId { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
