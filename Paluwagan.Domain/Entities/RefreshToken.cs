using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Domain.Entities
{
    public sealed class RefreshToken
    {
        public RefreshTokenId Id { get; protected set; } = default!;
        public Guid UserId { get; protected set; } = default!;
        public string Token { get; private set; } = string.Empty;
        public bool IsUsed { get; private set; } = false;
        public bool IsRevoked { get; private set; } = false;
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        protected RefreshToken() { }

        [SetsRequiredMembers]
        public RefreshToken(Guid userId, string token, DateTime createdAt)
        {
            Id = new RefreshTokenId(Guid.NewGuid());
            UserId = userId;
            Token = token;
            CreatedAt = createdAt;
            ExpiryDate = createdAt.AddDays(7);
        }

        public void MarkAsUsed(DateTime utcNow)
        {
            if (IsRevoked)
                throw new BusinessRuleBrokenException("Cannot use a revoked token.");

            if (IsUsed)
                throw new BusinessRuleBrokenException("Token has already been used.");

            if (ExpiryDate <= utcNow)
                throw new BusinessRuleBrokenException("Cannot use an expired token.");

            IsUsed = true;
        }

        public void Revoke()
        {
            if (IsRevoked) return;
            IsRevoked = true;
        }   
    }
}