using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paluwagan.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Paluwagan.Domain.Enums;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel.Exceptions;
using System.Diagnostics.CodeAnalysis;


namespace Paluwagan.Domain.Entities
{
    public sealed class ApplicationUser : IdentityUser<Guid>, IAggregateRoot
    {
        public AccountId AccountId { get; private set; } = default!;
        public string FullName { get; private set; } = string.Empty;
        public string? GCashNumber { get; private set; }
        public string? MayaNumber { get; private set; }
        public string? QrCodeUrl { get; private set; }
        public string? FcmToken { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void ClearDomainEvents() => _domainEvents.Clear();
        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        protected ApplicationUser() { }
        [SetsRequiredMembers]
        public ApplicationUser(string fullName, string email, UserRole role)
        {
            Id = Guid.NewGuid();
            AccountId = new AccountId("PAL-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant());
            FullName = fullName;
            UserName = email;
            Email = email;
            Role = role;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static ApplicationUser Create(string fullName, string email, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new BusinessRuleBrokenException("Full name is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new BusinessRuleBrokenException("Email is required.");

            return new ApplicationUser(fullName, email, role);
        }

        public void UpdatePaymentAccounts(string? gcash, string? maya)
        {
            GCashNumber = gcash;
            MayaNumber = maya;
        }

        public void UpdateQrCode(string? url)
        {
            QrCodeUrl = url;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateFcmToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new BusinessRuleBrokenException("FCM token is required.");
            FcmToken = token;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ClearFcmToken()
        {
            FcmToken = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void PromoteToOrganizer()
        {
            Role = UserRole.Organizer;
            UpdatedAt = DateTime.UtcNow;
        }
        public void AddRefreshToken(string token, DateTime utcNow)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new BusinessRuleBrokenException("Token is required.");

            if (utcNow == default)
                throw new BusinessRuleBrokenException("UTC time is required.");

            // Check if token already exists
            if (_refreshTokens.Any(r => r.Token == token))
                throw new BusinessRuleBrokenException("Token already exists.");

            var refreshToken = new RefreshToken(Id, token, utcNow);
            _refreshTokens.Add(refreshToken);
            UpdatedAt = DateTime.UtcNow;
        }

        public RefreshToken RotateRefreshToken(string oldTokenValue, string newTokenValue, DateTime utcNow)
        {
            if (string.IsNullOrWhiteSpace(oldTokenValue))
                throw new BusinessRuleBrokenException("Old token value is required.");

            if (string.IsNullOrWhiteSpace(newTokenValue))
                throw new BusinessRuleBrokenException("New token value is required.");

            if (utcNow == default)
                throw new BusinessRuleBrokenException("UTC time is required.");

            var oldToken = _refreshTokens.SingleOrDefault(r => r.Token == oldTokenValue);
            if (oldToken == null)
                throw new BusinessRuleBrokenException("Refresh token not found.");

            // REUSE DETECTION
            if (oldToken.IsUsed || oldToken.IsRevoked)
            {
                RevokeAllRefreshTokens();
                throw new BusinessRuleBrokenException("Token reuse detected. All sessions revoked.");
            }

            // EXPIRY CHECK
            if (oldToken.ExpiryDate < utcNow)
            {
                throw new BusinessRuleBrokenException("Refresh token has expired.");
            }

            // Update old token state
            oldToken.MarkAsUsed(utcNow);

            // Create new token
            var newRefreshToken = new RefreshToken(Id, newTokenValue, utcNow);
            _refreshTokens.Add(newRefreshToken);
            UpdatedAt = DateTime.UtcNow;

            return newRefreshToken;
        }

        public void RevokeSpecificToken(string tokenValue)
        {
            if (string.IsNullOrWhiteSpace(tokenValue))
                throw new BusinessRuleBrokenException("Token value is required.");

            var refreshToken = _refreshTokens.SingleOrDefault(r => r.Token == tokenValue);
            if (refreshToken == null) return;

            refreshToken.Revoke();
            UpdatedAt = DateTime.UtcNow;
        }

        public void RevokeAllRefreshTokens()
        {
            var activeTokens = _refreshTokens.Where(t => !t.IsRevoked && !t.IsUsed).ToList();
            foreach (var token in activeTokens)
            {
                token.Revoke();
            }
            UpdatedAt = DateTime.UtcNow;
        }
    }
}