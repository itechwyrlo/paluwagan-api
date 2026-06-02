using System.Diagnostics.CodeAnalysis;
using Paluwagan.Domain.Enums;
using Paluwagan.Domain.ValueObjects;
using Paluwagan.SharedKernel;
using Paluwagan.SharedKernel.Exceptions;

namespace Paluwagan.Domain.Entities
{
    public sealed class Notification : AggregateRoot
    {
        public NotificationId Id { get; private set; } = default!;
        public Guid UserId { get; private set; }
        public NotificationType Type { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public bool IsRead { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public string ReferenceId { get; private set; } = string.Empty;

        protected Notification() { }

        [SetsRequiredMembers]
        private Notification(Guid userId, NotificationType type, string title, string body, string referenceId)
        {
            Id = new NotificationId(Guid.NewGuid());
            UserId = userId;
            Type = type;
            Title = title;
            Body = body;
            IsRead = false;
            CreatedAt = DateTimeOffset.UtcNow;
            ReferenceId = referenceId;
        }

        public static Notification Create(Guid userId, NotificationType type, string title, string body, string referenceId)
        {
            if (userId == Guid.Empty)
                throw new BusinessRuleBrokenException("User ID is required.");

            if (string.IsNullOrWhiteSpace(title))
                throw new BusinessRuleBrokenException("Notification title is required.");

            if (string.IsNullOrWhiteSpace(body))
                throw new BusinessRuleBrokenException("Notification body is required.");

            if (string.IsNullOrWhiteSpace(referenceId))
                throw new BusinessRuleBrokenException("Reference ID is required.");

            return new Notification(userId, type, title, body, referenceId);
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
